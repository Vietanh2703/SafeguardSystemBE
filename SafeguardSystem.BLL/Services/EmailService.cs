using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.UnitOfWork;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;


namespace SafeguardSystem.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public const string logoUrl = "https://www.freepik.com/free-photos-vectors/safeguard-icon";

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger, IUnitOfWork unitOfWork)
        {
            // Inject EmailSettings từ appsettings.json
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        }

        public async Task SendActivationEmailAsync(EmailRequest emailRequest)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress("Safeguard System", _emailSettings.Sender);
            email.To.Add(MailboxAddress.Parse(emailRequest.Email));
            email.Subject = emailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = emailRequest.EmailBody;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Sender, _emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public string GenerateEmailBody(string FullName, string OtpText)
        {
            string body = string.Empty;
            body = "<div style='font-family: Arial, sans-serif;'>";
            body += "<div style='background-color: #f8f8f8; padding: 20px;'>";
            body += "<div style='background-color: #fff; padding: 20px; border-radius: 10px;'>";
            body += "<div style='text-align: left;'>";
            body += "<img src='" + logoUrl + "' alt='Safeguard System' style='width: 100px; height: 100px; display: block; margin-bottom: 20px;'>";
            body += "<h1 style='color: #333; font-size: 24px; margin-bottom: 20px;'>Safeguard System OTP Sender</h1>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Hi " + FullName + ",</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Your OTP is: <strong>" + OtpText + "</strong></p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Please use this OTP to activate your account.</p>";
            body += "</div>";
            body += "<div style='text-align: center; margin-top: 40px;'>"; // Increased margin-top to 40px
            body += "<p style='color: #999; font-size: 14px;'>© 2025 Safeguard Assignment & Management System. All rights reserved.</p>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";


            return body;

        }
    }   
}
