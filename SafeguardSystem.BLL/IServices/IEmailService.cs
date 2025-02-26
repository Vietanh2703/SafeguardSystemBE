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
        Task SendActivationEmailAsync(EmailRequest emailRequest);
        string GenerateEmailBody(string FullName, string OtpText);
    }
}
