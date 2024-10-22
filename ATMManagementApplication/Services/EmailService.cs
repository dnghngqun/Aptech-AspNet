using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace ATMManagementApplication.Services{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config){
            _config = config;
        }

        public void SendEmail(string toEmail, string Subject, string Content){
            var smtpConfig = _config.GetSection("Smtp");

            var smtpClient = new SmtpClient(smtpConfig["Host"]){
                Port = int.Parse(smtpConfig["Port"]),
                Credentials = new NetworkCredential(smtpConfig["Username"], smtpConfig["Password"]),
                EnableSsl = bool.Parse(smtpConfig["EnableSSL"]),
            };
            var mailMessage = new MailMessage{
                From = new MailAddress(smtpConfig["Username"]),
                Subject = Subject,
                Body = Content,
                IsBodyHtml = true, 
            };

            mailMessage.To.Add(toEmail);
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}