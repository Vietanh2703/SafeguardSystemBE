using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.Constants;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.Common.JWTSettings;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using static SafeguardSystem.BLL.Providers.JWTProvider;


namespace SafeguardSystem.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        // Đăng nhập bằng Email và Password
        public async Task<ResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var firebaseApiKey = _configuration["Firebase:ApiKey"];
            var firebaseUrl = _configuration["Firebase:Url"];

            if (string.IsNullOrEmpty(firebaseApiKey) || string.IsNullOrEmpty(firebaseUrl))
            {
                return new ResponseDTO("Cannot connect to Firebase Service", 400, false);
            }

            var payload = new
            {
                email = loginDTO.Account,
                password = loginDTO.Password,
                returnSecureToken = true
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsJsonAsync(firebaseUrl, payload);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Firebase login request failed: {ex.Message}", 500, false);
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ResponseDTO("Invalid email or password", 400, false);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(responseContent);
            var root = jsonDocument.RootElement;

            if (!root.TryGetProperty("localId", out var userIdElement) || string.IsNullOrEmpty(userIdElement.GetString()))
            {
                return new ResponseDTO("Cannot collect any data from Firebase", 400, false);
            }

            string firebaseUid = userIdElement.GetString() ?? string.Empty;
            string email = root.GetProperty("email").GetString() ?? string.Empty;
            string displayName = root.GetProperty("displayName").GetString() ?? email;

            // Kiểm tra user trong database
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(firebaseUid);
            if (user == null)
            {
                
                var defaultRole = await _unitOfWork.Roles.GetRoleIdByNameAsync(""); // Role mặc định nếu không có
                user = new User
                {
                    UserId = firebaseUid,
                    Email = email,
                    UserName = displayName,
                    FullName = displayName,
                    IsActive = true,
                    IsEmailConfirmed = true,
                    IsDeleted = false,
                    RoleID = defaultRole?.RoleId ?? Guid.NewGuid()
                };

                await _unitOfWork.Users.CreateUserAsync(user);
            }

            // Check nếu user không active, bị khóa, chưa xác nhận email hoặc đã bị xóa
            if (!user.IsActive || user.IsLocked || !user.IsEmailConfirmed || user.IsDeleted)
            {
                if (user.IsDeleted || !user.IsActive || user.IsLocked)
                {
                    return new ResponseDTO("This account does not exist.", 400, false);
                }
                if (!user.IsEmailConfirmed)
                {
                    return new ResponseDTO("Your account is not verified, please check your email.", 400, false);
                }
            }

            var role = await _unitOfWork.Roles.GetByGuIdAsync(user.RoleID);
            var roleName = role?.RoleName ?? ""; // Mặc định nếu không có role

            var claims = new List<Claim>
    {
        new Claim(JwtConstant.KeyClaim.Email, user.Email ?? string.Empty),
        new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()),
        new Claim(JwtConstant.KeyClaim.fullName, user.FullName ?? string.Empty),
        new Claim(JwtConstant.KeyClaim.Role, roleName)
    };

            var refreshTokenKey = JwtProvider.GenerateRefreshToken(claims);
            var accessTokenKey = JwtProvider.GenerateAccessToken(claims);

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

            return new ResponseDTO("Login successful", 200, true, new
            {
                AccessToken = accessTokenKey,
                RefreshToken = refreshToken.RefreshTokenKey,
                Email = user.Email,
                FullName = user.FullName,
                Role = roleName
            });
        }

        //Đăng nhập bằng Google
        public async Task<ResponseDTO> SignInWithGoogleAsync(GoogleLoginDTO googleLoginDTO)
        {
            // Xác thực Google token bằng Firebase Admin SDK
            FirebaseToken decodedToken;
            try
            {
                decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(googleLoginDTO.IdToken);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error verifying Google token: {ex.Message}", 400, false);
            }

            // Lấy thông tin từ token
            string userId = decodedToken.Uid;
            string email = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"].ToString() : null;
            string name = decodedToken.Claims.ContainsKey("name") ? decodedToken.Claims["name"].ToString() : null;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
                return new ResponseDTO("Invalid Google token", 400, false);

            // Kiểm tra user trong database
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
            if (user == null)
            {
                // Gán vai trò mặc định Processing nếu user mới
                var processingRole = await _unitOfWork.Roles.GetRoleIdByNameAsync("Processing");

                user = new User
                {
                    UserId = userId,
                    Email = email,
                    UserName = name ?? email,
                    FullName = name,
                    IsActive = false,
                    IsEmailConfirmed = false,
                    IsDeleted = false,
                    IsLocked = false,
                    Phone = "",
                    Avatar = "https://www.veryicon.com/icons/miscellaneous/generic-icon-3/avatar-real.html",
                    RoleID = processingRole.RoleId,
                };
                await _unitOfWork.Users.CreateUserAsync(user);

                // Tạo LoginRequest cho Admin
                var loginRequest = new LoginRequest
                {
                    RequestId = Guid.NewGuid(),
                    UserId = user.UserId,
                    Email = user.Email,
                    DateSent = DateTime.UtcNow,
                    Status = "PENDING",
                    Reason = "Google login request"
                };
                _unitOfWork.LoginRequests.Add(loginRequest);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Your account is pending approval. Please wait for admin approval.", 200, true);
            }

            // Kiểm tra nếu user bị khóa hoặc bị xóa khỏi hệ thống
            if (user.IsLocked || user.IsDeleted)
            {
                return new ResponseDTO("Your account does not exist.", 400, false);
            }

            // Kiểm tra và thu hồi RefreshToken cũ nếu có
            var existingRefreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByUserID(user.UserId);
            if (existingRefreshToken != null)
            {
                existingRefreshToken.IsRevoked = true;
                await _unitOfWork.RefreshTokens.UpdateAsync(existingRefreshToken);
            }

            // Lấy role của user
            var role = await _unitOfWork.Roles.GetByGuIdAsync(user.RoleID);
            var roleName = role?.RoleName ?? ""; // Mặc định BusinessPartner

            // Khởi tạo danh sách claims
            var claims = new List<Claim>
    {
        new Claim(JwtConstant.KeyClaim.Email, user.Email ?? string.Empty),
        new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()),
        new Claim(JwtConstant.KeyClaim.fullName, user.FullName ?? string.Empty),
        new Claim(JwtConstant.KeyClaim.Role, roleName)
    };

            // Tạo refresh token và access token mới
            var refreshTokenKey = JwtProvider.GenerateRefreshToken(claims);
            var accessTokenKey = JwtProvider.GenerateAccessToken(claims);

            // Lưu refresh token mới
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

            // Trả về dữ liệu theo đúng format của LoginAsync
            return new ResponseDTO("Login successful", 200, true, new
            {
                AccessToken = accessTokenKey,
                RefreshToken = refreshToken.RefreshTokenKey,
                Email = user.Email,
                FullName = user.FullName,
                Role = roleName
            });
        }

        // Đăng xuất
        public async Task<ResponseDTO> LogoutAsync(LogoutDTO logoutDTO)
        {
            //Tìm refresh token trong database
            var refreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByKey(logoutDTO.Token);

            // Kiểm tra xem refresh token có tồn tại không
            if (refreshToken == null)
            {
                return new ResponseDTO("Refresh token not found", 404, false);
            }
            try
            {
                // Thu hồi refresh token
                await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(logoutDTO.Token);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error during logout: {ex.Message}", 500, false);
            }
            return new ResponseDTO("Logout successful", 200, true);
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

            //khởi tạo claim
            var claims = new List<Claim>
            {
                new Claim(JwtConstant.KeyClaim.Email, user.Email ?? string.Empty),
                new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()),
                new Claim(JwtConstant.KeyClaim.fullName, user.FullName ?? string.Empty)
            };

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
