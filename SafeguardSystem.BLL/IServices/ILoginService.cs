using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface ILoginService
    {
        Task<ResponseDTO> Login(LoginDTO loginDTO);
        Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey);
        Task<ResponseDTO> LogoutAsync(string refreshTokenKey);
    }
}
