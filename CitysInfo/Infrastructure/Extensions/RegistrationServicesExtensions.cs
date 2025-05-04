using Asp.Versioning;

namespace CitysInfo.Infrastructure.Extensions
{
    public static class RegistrationServicesExtensions
    {
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
