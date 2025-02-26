using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface IEmailService
    {
        Task SendActivationEmailAsync(string email, string activationLink);
    }
}
