using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CitysInfo;
using CitysInfo.DbContexts;
using CitysInfo.Infrastructure.Extensions;
using CitysInfo.Repositories;
using CitysInfo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;


// تنظیم Serilog
Log.Logger = new LoggerConfiguration()
     .MinimumLevel.Debug()
     .WriteTo.Console() //نمایش لاگ‌ها در کنسول
                        //.ReadFrom.Configuration(builder.Configuration) //خواندن تنظیمات از appsettings.json             
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day) // ذخیره لاگ‌ها در فایل و تبدیل فایل جدید لاگ به صورت روزانه
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(); // فعال‌سازی Serilog

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.SetMinimumLevel(LogLevel.Information);


// Add services to the container.

#region Support Xml
builder.Services.AddControllers(option =>
{
    ////if we want to change the json
    #region change json
    ////اگر بخواهیم ورودی و خروجی پیش فرض را که 
    ////application/json
    ////است را تغییر دهیم از پراپرتی های زیر استفاده میکنیم
    //option.OutputFormatters.Add(formaters => ...);
    //option.InputFormatters.Add(formaters =>...);
    #endregion
    option.ReturnHttpNotAcceptable = true; // برگرداندن 406 برای فرمت‌های غیرقابل پشتیبانی
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters() // افزودن پشتیبانی از XML
;
#endregion

builder.Services.AddProblemDetails();
////شخصی‌سازی پاسخ‌های خطا در پروژه های بزرگ
//builder.Services.AddProblemDetails(option =>
//{
//    option.CustomizeProblemDetails = ctx =>
//    {
//        ctx.ProblemDetails.Extensions.Add("additionalInfo",
//            "Additional info example");
//        ctx.ProblemDetails.Extensions.Add("server",
//            Environment.MachineName);
//    };
//});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddOpenApi();


builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoDbContext>(option =>
    option.UseSqlite(
    builder.Configuration.GetConnectionString("CityInfoDBConnectionString")
    //builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]
    )
);

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// اعتبارسنجی توکن ایجاد شده
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            //ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
               Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFormAhwaz", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Ahwaz");
    });
});

builder.Services.AddCustomApiVersioning();


////*************************************
// راه حل اول
//builder.Services.AddSwaggerGen(options =>
//{
//    // خواندن خودکار توضیحات XML (اختیاری)
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    options.IncludeXmlComments(xmlPath);
//});
////*************************************

////*************************************
////راه حل دوم
// فقط در صورتی که واقعاً ضروری است!
//builder.Services.AddOptions<SwaggerGenOptions>()
//    .Configure<IApiVersionDescriptionProvider>((setupAction, apiVersionDescriptionProvider) =>
//    {
//        foreach (var description in
//        apiVersionDescriptionProvider.ApiVersionDescriptions)
//        {
//            setupAction.SwaggerDoc(
//                $"{description.GroupName}",
//                new()
//                {
//                    Title = "City Info API",
//                    Version = description.ApiVersion.ToString(),
//                    Description = "Through this API you can access cities and their points of interest.",
//                });
//        }

//        var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//        var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

//        setupAction.IncludeXmlComments(xmlCommentsPath);
//    });
//builder.Services.AddSwaggerGen();
////*************************************

////*************************************
////راه حل سوم
var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
    .GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
    foreach (var description in
        apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc(
            $"{description.GroupName}",
            new()
            {
                Title = "City Info API",
                Version = description.ApiVersion.ToString(),
                Description = "Through this API you can access cities and their points of interest.",
            });
    }
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsPath);

    // تعریف امنیتی
    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http, // نوع احراز هویت (HTTP-based)
        Scheme = "Bearer", // استفاده از توکن Bearer
        Description = "Input a valid token to access this API",
        In = ParameterLocation.Header,
        BearerFormat = "JWT"
    });

    // اعمال امنیتی
    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference //به تعریف امنیتی ایجاد شده در AddSecurityDefinition ارجاع می‌دهد.
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth",
                }
            },
            new List<string>()
            // or
            // Array.Empty<string>()
            // or
            // []
        }
    });

    // سایر تنظیمات (مثل XML Comments)...
});
////*************************************   

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
    | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var desc in descriptions)
        {
            setupAction.SwaggerEndpoint(
                $"/swagger/{desc.GroupName}/swagger.json",
                desc.GroupName.ToUpperInvariant());
        }
    });
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
