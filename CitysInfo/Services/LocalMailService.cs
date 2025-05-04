using System.Net;
using System.Net.Mail;

namespace CitysInfo.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSetting:mailToAddress"];
            _mailFrom = configuration["mailSetting:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail From {_mailFrom} To {_mailTo} ,"
                + $"with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject {subject}");
            Console.WriteLine($"Message {message}");
        }

        //public static void Email(string subject,string htmlString,string to)
        //{
        //    try
        //    {
        //         string _mailFrom = "noreplay@gmail.com";
        //        MailMessage message = new MailMessage();
        //        SmtpClient smtpClient = new SmtpClient();
        //        message.From = new MailAddress("FromMailAddress");
        //        message.To.Add(new MailAddress(to));
        //        message.Subject = subject;
        //        message.IsBodyHtml = true;  //to make message body as html
        //        message.Body = htmlString;
        //        smtpClient.Port = 587;
        //        smtpClient.Host = to; //for gmail host
        //        smtpClient.EnableSsl = true;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = new NetworkCredential("FromMailAddress", "password");
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.Send(message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
