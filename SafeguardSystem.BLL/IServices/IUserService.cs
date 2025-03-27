using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IUserService
{
    Task<ResponseDTO> CreateUserAsync(CreateUserDTO createUserDTO);
    Task<ResponseDTO> VerifyOtpAsync(string Email, OtpDTO otpDTO);
    Task<ResponseDTO> RefreshOtpAsync(string email);
    Task<ResponseDTO> UpdatePasswordAsync(string email, UpdatePasswordDTO updatePasswordDTO);
    Task<ResponseDTO> UpdateUserAsync(string UserId, UpdateUserDTO updateUserDTO);
    Task<ResponseDTO> UpdateAvatarAsync(string userId, AvatarDTO avatarDTO);
    Task<ResponseDTO> DeleteUserAsync(string userId);
    Task<ResponseDTO> GetAllUsersAsync(int pageIndex, int pageSize);
    Task<ResponseDTO> GetAllUsersAsync();
    Task<ResponseDTO> GetUsersByRoleAsync(string roleName);
    Task<ResponseDTO> GetAllRolesAsync();
    Task<ResponseDTO> GetUserByUserIdAsync(string userId);
    Task<ResponseDTO> BanUserAsync(string userId);
    Task<ResponseDTO> UnbanUserAsync(string userId);
}