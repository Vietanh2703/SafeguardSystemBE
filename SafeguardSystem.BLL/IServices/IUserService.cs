using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface IUserService
    {
        Task<ResponseDTO> CreateUserAsync(CreateUserDTO createUserDTO);
        Task<ResponseDTO> VerifyOtpAsync(OtpDTO otpDTO);
        //Task<ResponseDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO);
        Task<ResponseDTO> DeleteUserAsync(string userId);
        //Task<ResponseDTO> GetUserAsync(string userId);
        Task<ResponseDTO> GetAllUsersAsync(int pageIndex, int pageSize);
    }
}
