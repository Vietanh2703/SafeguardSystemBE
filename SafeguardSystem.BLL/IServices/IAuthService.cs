using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IAuthService
{
    Task<ResponseDTO> LoginAsync(LoginDTO loginDTO);
    Task<ResponseDTO> SignInWithGoogleAsync(GoogleLoginDTO googleLoginDTO);
    Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey);
    Task<ResponseDTO> LogoutAsync(string refreshTokenKey);
}