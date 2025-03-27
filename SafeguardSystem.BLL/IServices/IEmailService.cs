using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest emailRequest);
    string GenerateWelcomeEmailBody(string FullName, string Email, string Password);
    string GenerateWelcomeGoogleLoginUser(string Email);
    string GenerateOtpEmailBody(string FullName, string OtpText);
    string GenerateActivationSuccessEmailBody(string FullName);
    string GenerateBanUserEmailBody(string FullName);
    string GenerateUnbanUserEmailBody(string FullName);
    string GenerateAcceptedReportEmail(string sender, string respondent, string reason, DateTime date);
    string GenerateRejectedReportEmail(string sender, string respondent, string reason, DateTime date);
}