using System.Net;
using System.Net.Mail;

namespace CitysInfo.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSetting:mailToAddress"];
            _mailFrom = configuration["mailSetting:mailFromAddress"];
        }
        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail From {_mailFrom} To {_mailTo} ,"
                + $"with {nameof(CloudMailService)}");
            Console.WriteLine($"Subject {subject}");
            Console.WriteLine($"Message {message}");
        }
    }
}
