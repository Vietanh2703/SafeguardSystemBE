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

        public async Task SendEmailAsync(EmailRequest emailRequest)
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

        public string GenerateWelcomeEmailBody(string FullName, string Email, string Password)
        {
            var LoginUrl = "http://localhost:5173/login";
            string body = string.Empty;
            body = "<div style='font-family: Arial, sans-serif;'>";
            body += "<div style='background-color: #f8f8f8; padding: 20px;'>";
            body += "<div style='background-color: #fff; padding: 20px; border-radius: 10px;'>";
            body += "<div style='text-align: left;'>";
            body += "<img src='" + logoUrl + "' alt='Safeguard System' style='width: 100px; height: 100px; display: block; margin-bottom: 20px;'>";
            body += "<h1 style='color: #333; font-size: 24px; margin-bottom: 20px;'>Welcome to Safeguard System &#127881;</h1>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Hi " + FullName + ",</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Welcome to Safeguard Assignment & Management System! We’re excited to have you on board. Below are your login credentials to access our system:</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'><strong>&#9679; Email:</strong> " + Email + "</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'><strong>🔹 Temporary Password:</strong> " + Password + "</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>For security reasons, please log in as soon as possible and change your password.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Next Steps:</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>1. Click the button below to log in.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>2. Upon login, you’ll be prompted to update your password.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>3. Explore your dashboard and start using the system!</p>";
            body += "<div style='text-align: center; margin: 20px 0;'>";
            body += "<a href='" + LoginUrl + "' style='background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Log in to Your Account</a>";
            body += "</div>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>If you have any questions or need assistance, feel free to reach out to our support team at vietanhcodega123@gmail.com.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Welcome aboard! &#128640;</p>";
            body += "</div>";
            body += "<div style='text-align: center; margin-top: 40px;'>";
            body += "<p style='color: #999; font-size: 14px;'>© 2025 Safeguard Assignment & Management System. All rights reserved.</p>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            return body;
        }

        public string GenerateWelcomeGoogleLoginUser(string Email)
        {
            var LoginUrl = "http://localhost:5173/login";
            string body = string.Empty;
            body = "<div style='font-family: Arial, sans-serif;'>";
            body += "<div style='background-color: #f8f8f8; padding: 20px;'>";
            body += "<div style='background-color: #fff; padding: 20px; border-radius: 10px;'>";
            body += "<div style='text-align: left;'>";
            body += "<img src='" + logoUrl + "' alt='Safeguard System' style='width: 100px; height: 100px; display: block; margin-bottom: 20px;'>";
            body += "<h1 style='color: #333; font-size: 24px; margin-bottom: 20px;'>Welcome to Safeguard System &#127881;</h1>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Hi " + Email + ",</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Welcome to Safeguard Assignment & Management System! We’re excited to have you on board. Below are your login credentials to access our system:</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Next Steps:</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>1. Click the button below to log in.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>2. Explore your dashboard and start using the system!</p>";
            body += "<div style='text-align: center; margin: 20px 0;'>";
            body += "<a href='" + LoginUrl + "' style='background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Log in to Your Account</a>";
            body += "</div>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>If you have any questions or need assistance, feel free to reach out to our support team at vietanhcodega123@gmail.com.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Welcome aboard! &#128640;</p>";
            body += "</div>";
            body += "<div style='text-align: center; margin-top: 40px;'>";
            body += "<p style='color: #999; font-size: 14px;'>© 2025 Safeguard Assignment & Management System. All rights reserved.</p>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";

            return body;
        }

        public string GenerateRejectGoogleLoginUser(string Email)
        {
            string body = string.Empty;
            body = "<div style='font-family: Arial, sans-serif;'>";
            body += "<div style='background-color: #f8f8f8; padding: 20px;'>";
            body += "<div style='background-color: #fff; padding: 20px; border-radius: 10px;'>";
            body += "<div style='text-align: left;'>";
            body += "<img src='" + logoUrl + "' alt='Safeguard System' style='width: 100px; height: 100px; display: block; margin-bottom: 20px;'>";
            body += "<h1 style='color: #333; font-size: 24px; margin-bottom: 20px;'>Safeguard System Automatic Sender</h1>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Dear " + Email + ",</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Thank you for your interest in registering with our system. Unfortunately, we were unable to process your email " + Email + " for registration at this time.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>This may be due to one of the following reasons:</p>";
            body += "</div>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>- The email address does not meet our system's requirements.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>- There was an issue verifying the email.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>If you believe this was a mistake or need further assistance, please feel free to contact our support team at vietanhcodega1234@gmail.com.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>We appreciate your understanding.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Best regards,<br>Việt Anh<br>Coder bị dí deadline sml.<br>Safeguard Assignment & Management System.</p>";
            body += "<div style='text-align: center; margin-top: 40px;'>"; // Increased margin-top to 40px
            body += "<p style='color: #999; font-size: 14px;'>© 2025 Safeguard Assignment & Management System. All rights reserved.</p>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            return body;
        }

        public string GenerateOtpEmailBody(string FullName, string OtpText)
        {
            string body = string.Empty;
            body = "<div style='font-family: Arial, sans-serif;'>";
            body += "<div style='background-color: #f8f8f8; padding: 20px;'>";
            body += "<div style='background-color: #fff; padding: 20px; border-radius: 10px;'>";
            body += "<div style='text-align: left;'>";
            body += "<img src='" + logoUrl + "' alt='Safeguard System' style='width: 100px; height: 100px; display: block; margin-bottom: 20px;'>";
            body += "<h1 style='color: #333; font-size: 24px; margin-bottom: 20px;'>Safeguard System Automatic Sender</h1>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Hi " + FullName + ",</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Your OTP is: <strong>" + OtpText + "</strong></p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Please use this OTP to activate your account and don't send it to anyone.</p>";
            body += "</div>";
            body += "<div style='text-align: center; margin-top: 40px;'>"; // Increased margin-top to 40px
            body += "<p style='color: #999; font-size: 14px;'>© 2025 Safeguard Assignment & Management System. All rights reserved.</p>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";


            return body;

        }

        public string GenerateActivationSuccessEmailBody(string FullName)
        {
            string body = string.Empty;
            body = "<div style='font-family: Arial, sans-serif;'>";
            body += "<div style='background-color: #f8f8f8; padding: 20px;'>";
            body += "<div style='background-color: #fff; padding: 20px; border-radius: 10px;'>";
            body += "<div style='text-align: left;'>";
            body += "<img src='" + logoUrl + "' alt='Safeguard System' style='width: 100px; height: 100px; display: block; margin-bottom: 20px;'>";
            body += "<h1 style='color: #333; font-size: 24px; margin-bottom: 20px;'>Safeguard System Automatic Sender</h1>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Hi " + FullName + ",</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>Your account has been activated successfully.</p>";
            body += "<p style='color: #333; font-size: 12px; margin-bottom: 10px;'>You can now login to the system.</p>";
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
