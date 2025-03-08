using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
        string GenerateWelcomeEmailBody(string FullName, string Email, string Password);
        string GenerateWelcomeGoogleLoginUser(string Email);
        string GenerateOtpEmailBody(string FullName, string OtpText);
        string GenerateActivationSuccessEmailBody(string FullName);
        string GenerateBanUserEmailBody(string FullName);
        string GenerateUnbanUserEmailBody(string FullName);
    }
}
