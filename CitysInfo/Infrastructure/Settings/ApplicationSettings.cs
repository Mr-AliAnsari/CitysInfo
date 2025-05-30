namespace CitysInfo.Infrastructure.Settings
{
    public class ApplicationSettings
    {
        public const string KeyName = nameof(ApplicationSettings);

        public required IdentitySettings IdentitySettings { get; set; }
    }
}
