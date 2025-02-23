using SafeguardSystem.BLL.IServices;
using SafeguardSystem.BLL.Providers;
using SafeguardSystem.Common.Constants;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.Common.JWTSettings;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SafeguardSystem.BLL.Services.LoginService;

namespace SafeguardSystem.BLL.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Đăng nhập
        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {

            //kiểm tra người dùng
            var user = await _unitOfWork.Users.GetUserByEmailAsync(loginDTO.Account);
            if (user == null)
            {
                return new ResponseDTO("User not found", 404, false);
            }

            Console.WriteLine($"Stored Hash: {user.PasswordHash}");
            string password = "123";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine($"New Hashed Password: {hashedPassword}");


            // kiểm tra mật khẩu
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new ResponseDTO("Invalid email or password.", 400, false);
            }

            //kiểm tra refreshToken
            var exitsRefreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByUserID(user.UserId);
            if (exitsRefreshToken != null)
            {
                // nếu có thì thu hồi
                exitsRefreshToken.IsRevoked = true;
                await _unitOfWork.RefreshTokens.UpdateAsync(exitsRefreshToken); // cập nhật
            }
            //khởi tạo claim
            var claims = new List<Claim>();

            //thêm email
            claims.Add(new Claim(JwtConstant.KeyClaim.Email, user.Email));

            //thêm id
            claims.Add(new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()));

            //thêm name
            claims.Add(new Claim(JwtConstant.KeyClaim.fullName, user.FullName));

            //tạo refesh token
            var refreshTokenKey = JwtProvider.GenerateRefreshToken(claims);

            //tạo access token
            var accessTokenKey = JwtProvider.GenerateAccessToken(claims);

            //new refreshToken model
            var refreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                RefreshTokenKey = refreshTokenKey,
                IsRevoked = false,
                CreateAt = DateTime.UtcNow
            };
            _unitOfWork.RefreshTokens.Add(refreshToken);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error saving refresh token: {ex.Message}", 500, false);
            }
            // Kiểm tra RoleID hợp lệ trước
            if (user.RoleID == null)
            {
                return new ResponseDTO("User role is missing", 400, false, null);
            }

            // Truy vấn role từ database (đảm bảo phương thức là async)
            var role = await _unitOfWork.Roles.GetByGuidAsync(user.RoleID);

            if (role == null)
            {
                return new ResponseDTO("Role not found", 400, false, null);
            }

            var roleName = role.RoleName;

            // Trả response
            return new ResponseDTO("Login successful", 200, true, new
            {
                AccessToken = accessTokenKey,
                RefeshToken = refreshTokenKey,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = roleName,
            });


        }

        // Refresh token
        public async Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey)
        {
            // Kiểm tra tính hợp lệ của refresh token
            var claimsPrincipal = JwtProvider.Validation(refreshTokenKey);
            if (claimsPrincipal == null)
            {
                return new ResponseDTO("Invalid refresh token", 400, false);
            }

            // Lấy đối tượng RefreshToken từ refresh token Key
            var refreshTokenDTO = await _unitOfWork.RefreshTokens.GetRefreshTokenByKey(refreshTokenKey);
            if (refreshTokenDTO == null || refreshTokenDTO.IsRevoked)
            {
                return new ResponseDTO("Refresh token not found or has been revoked", 403, false);
            }

            // Kiểm tra nếu refresh token đã hết hạn
            var tokenExpirationDate = refreshTokenDTO.CreateAt?.AddDays(JWTSettingModel.ExpireDayRefreshToken);
            if (tokenExpirationDate == null || DateTime.UtcNow > tokenExpirationDate)
            {
                return new ResponseDTO("Refresh token expired, please login again", 403, false);
            }

            // Lấy thông tin người dùng từ UserId
            var user = await _unitOfWork.Users.GetByIdAsync(refreshTokenDTO.UserId);
            if (user == null)
            {
                return new ResponseDTO("User not found", 404, false);
            }

            // Khởi tạo danh sách claims
            var claims = new List<Claim>();

            // Thêm email vào claims
            claims.Add(new Claim(JwtConstant.KeyClaim.Email, user.Email));



            // Thêm UserId vào claims
            claims.Add(new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()));


            // Tạo access token mới
            var newAccessToken = JwtProvider.GenerateAccessToken(claims);

            // Lưu refresh token mới vào database
            var newRefreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                RefreshTokenKey = refreshTokenKey,
                IsRevoked = false,
                CreateAt = DateTime.UtcNow // Lưu thời gian tạo
            };

            // Xóa refresh token cũ
            _unitOfWork.RefreshTokens.Delete(refreshTokenDTO);
            // Thêm refresh token mới
            _unitOfWork.RefreshTokens.Add(newRefreshToken);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error refreshing tokens: {ex.Message}", 500, false);
            }

            return new ResponseDTO("Token refreshed successfully", 200, true);
        }

        // Đăng xuất
        public async Task<ResponseDTO> LogoutAsync(string refreshTokenKey)
        {
            // Tìm refresh token trong cơ sở dữ liệu
            var refreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByKey(refreshTokenKey);

            // Kiểm tra xem refresh token có tồn tại không
            if (refreshToken == null)
            {
                return new ResponseDTO("Refresh token not found", 404, false);
            }

            // Đánh dấu refresh token là đã thu hồi
            refreshToken.IsRevoked = true;
            _unitOfWork.RefreshTokens.UpdateAsync(refreshToken); // Cập nhật trạng thái token

            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error during logout: {ex.Message}", 500, false);
            }

            return new ResponseDTO("Logout successful", 200, true);
        }
    }
}
