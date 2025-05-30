using Asp.Versioning;
using CitysInfo.DbContexts;
using CitysInfo.Domain_Models.User;
using CitysInfo.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CitysInfo.Infrastructure.Extensions
{
    public static class RegistrationServicesExtensions
    {
        public static void RegisterServices
            (this IServiceCollection services, IConfiguration configuration, ApplicationSettings applicationSettings)
        {
            ArgumentNullException.ThrowIfNull(applicationSettings);

            services.AddControllers();

            services.AddDbContext<CityInfoDbContext>(options =>
            {
                options.UseSqlite
                    (configuration.GetConnectionString("CityInfoDBConnectionString"));
            });

            services.Configure<ApplicationSettings>
                (configuration.GetSection(ApplicationSettings.KeyName));

            services.AddCustomIdentity(applicationSettings.IdentitySettings);
        }

        public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings? identitySettings)
        {
            // اگر نال بود
            ArgumentNullException.ThrowIfNull(identitySettings);

            services.Configure<SecurityStampValidatorOptions>(option =>
            {
                option.ValidationInterval = TimeSpan.FromSeconds(1);
            });

            services.AddIdentity<User, Role>(identityOptions =>
            {
                //Password Settings
                identityOptions.Password.RequireDigit = identitySettings!.PasswordRequireDigit;
                identityOptions.Password.RequiredLength = identitySettings!.PasswordRequiredLength;
                identityOptions.Password.RequireNonAlphanumeric = identitySettings!.PasswordRequireNonAlphanumeric;
                identityOptions.Password.RequireUppercase = identitySettings!.PasswordRequireUppercase;
                identityOptions.Password.RequireLowercase = identitySettings!.PasswordRequireLowercase;

                //UserName Settings
                identityOptions.User.RequireUniqueEmail = identitySettings!.RequireUniqueEmail;

                //Singin Settings
                identityOptions.SignIn.RequireConfirmedEmail = identitySettings.RequireConfirmedEmail;
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = false;

                //Lockout Settings
                //identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                //identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //identityOptions.Lockout.AllowedForNewUsers = false;
            })
            .AddEntityFrameworkStores<CityInfoDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(setupAction =>
            {
                //وقتی این گزینه true باشد، API در پاسخ‌های خود لیست نسخه‌های موجود را در هدر
                //api-supported-versions قرار می‌دهد.
                setupAction.ReportApiVersions = true;
                //اگر کلاینت نسخه API را مشخص نکرده باشد، به طور پیش‌فرض از نسخه‌ای که در
                //DefaultApiVersion تعیین شده استفاده می‌شود.
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                //حتماً باید AssumeDefaultVersionWhenUnspecified برابر با true باشد
                setupAction.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);

                setupAction.ApiVersionReader = new UrlSegmentApiVersionReader();
                //setupAction.ApiVersionReader = ApiVersionReader.Combine(
                //new HeaderApiVersionReader("X-Version"),
                //new MediaTypeApiVersionReader("ver")
                //);
            })
            .AddApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'VVV";
                setupAction.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
