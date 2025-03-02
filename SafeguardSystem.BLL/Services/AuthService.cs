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
            //Lấy API để cấu hình Firebase
            var firebaseApiKey = _configuration["Firebase:ApiKey"];
            var firebaseUrl = _configuration["Firebase:Url"];

            if (string.IsNullOrEmpty(firebaseApiKey) || string.IsNullOrEmpty(firebaseUrl))
            {
                return new ResponseDTO("Cannot connect to Firebase Service", 400, false);
            }

            //Gọi REST API của Firebase để login
            var firebaseLoginUrl = $"{firebaseUrl}";

            var payload = new
            {
                email = loginDTO.Account,
                password = loginDTO.Password,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(firebaseUrl, payload);
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
            string displayName = root.GetProperty("displayName").GetString() ?? string.Empty; // có thể null

            // Kiểm tra User trong database, nếu chưa có thì tạo mới
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(firebaseUid);
            if (user == null)
            {
                user = new User
                {
                    UserId = firebaseUid,
                    Email = email,
                    UserName = displayName ?? email,
                    FullName = displayName,
                    IsActive = true,
                    IsEmailConfirmed = true,
                    IsDeleted = false,
                    RoleID = Guid.NewGuid() // Thiết lập Role mặc định hoặc lấy từ cấu hình
                };

                await _unitOfWork.Users.CreateUserAsync(user);
            }

            var exitsRefreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByUserID(user.UserId);
            if (exitsRefreshToken != null)
            {
                // nếu có thì thu hồi
                exitsRefreshToken.IsRevoked = true;
                await _unitOfWork.RefreshTokens.UpdateAsync(exitsRefreshToken); // cập nhật
            }

            var role = await _unitOfWork.Roles.GetByGuIdAsync(user.RoleID);
            var roleName = role.RoleName;

            //khởi tạo claim
            var claims = new List<Claim>
            {
                new Claim(JwtConstant.KeyClaim.Email, user.Email ?? string.Empty),
                new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()),
                new Claim(JwtConstant.KeyClaim.fullName, user.FullName ?? string.Empty)
            };
            if (roleName == "Admin")
                claims.Add(new Claim(JwtConstant.KeyClaim.Role, user.Role?.RoleName ?? "Admin"));
            else if (roleName == "Manager")
                claims.Add(new Claim(JwtConstant.KeyClaim.Role, user.Role?.RoleName ?? "Manager"));
            else if (roleName == "Security Guard")
                claims.Add(new Claim(JwtConstant.KeyClaim.Role, user.Role?.RoleName ?? "Security Guard"));

            //tạo refesh token
            var refreshTokenKey = JwtProvider.GenerateRefreshToken(claims);

            //tạo access token
            var accessTokenKey = JwtProvider.GenerateAccessToken(claims);

            //Cập nhật mới refreshToken
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

            

            // Tạo JWT token cục bộ cho ứng dụng
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

            // Kiểm tra các thông tin trong token
            string UserId = decodedToken.Uid;
            string Email = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"].ToString() : null;
            string Name = decodedToken.Claims.ContainsKey("name") ? decodedToken.Claims["name"].ToString() : null;

            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Email))
                return new ResponseDTO("Invalid Google token", 400, false);

            // Kiểm tra User trong database, nếu chưa có thì tạo mới
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(UserId);
            if (user == null)
            {
                user = new User
                {
                    UserId = UserId,
                    Email = Email,
                    UserName = Name ?? Email,
                    FullName = Name,
                    IsActive = true,
                    IsEmailConfirmed = true,
                    IsDeleted = false,
                    Phone = "",
                    Avatar = "https://www.veryicon.com/icons/miscellaneous/generic-icon-3/avatar-real.html",
                    RoleID = (await _unitOfWork.Roles.GetRoleIdByNameAsync("Business Partner")).RoleId, // Business Role
                };
                await _unitOfWork.Users.CreateUserAsync(user);

                // Tạo mới Business
                var business = new Business
                {
                    BusinessId = Guid.NewGuid(),
                    Name = user.FullName,
                    IsActive = true,
                    UserId = user.UserId,
                    ContractExpiry = DateTime.UtcNow.AddYears(1), // Example expiry date
                    IsDeleted = false
                };
                await _unitOfWork.Businesses.CreateAsync(business);
            }
            else
            {
                // Kiểm tra xem Business đã tồn tại chưa
                var existingBusiness = await _unitOfWork.Businesses.GetBusinessByUserIdAsync(user.UserId);
                if (existingBusiness == null)
                {
                    // Tạo mới Business nếu chưa tồn tại
                    var business = new Business
                    {
                        BusinessId = Guid.NewGuid(),
                        Name = user.FullName,
                        IsActive = true,
                        UserId = user.UserId,
                        ContractExpiry = DateTime.UtcNow.AddYears(1), // Example expiry date
                        IsDeleted = false
                    };
                    await _unitOfWork.Businesses.CreateAsync(business);
                }
            }

            var exitsRefreshToken = await _unitOfWork.RefreshTokens.GetRefreshTokenByUserID(user.UserId);
            if (exitsRefreshToken != null)
            {
                // nếu có thì thu hồi
                exitsRefreshToken.IsRevoked = true;
                await _unitOfWork.RefreshTokens.UpdateAsync(exitsRefreshToken); // cập nhật
            }

            // khởi tạo claim
            var claims = new List<Claim>
            {
                new Claim(JwtConstant.KeyClaim.Email, user.Email ?? string.Empty),
                new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()),
                new Claim(JwtConstant.KeyClaim.fullName, user.FullName ?? string.Empty)
            };

            if (user.Role.RoleName != null && user.Role.RoleName == "Business Partner")
            {
                    claims.Add(new Claim(JwtConstant.KeyClaim.Role, user.Role?.RoleName ?? "Business Partner"));
            }
            else
            {
                claims.Add(new Claim(JwtConstant.KeyClaim.Role, "Business Partner")); // Giá trị mặc định nếu không có vai trò
            }

            // tạo refesh token
            var refreshTokenKey = JwtProvider.GenerateRefreshToken(claims);

            // tạo access token
            var accessTokenKey = JwtProvider.GenerateAccessToken(claims);

            // Cập nhật mới refreshToken
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

            var role = await _unitOfWork.Roles.GetByGuIdAsync(user.RoleID);
            var roleName = role.RoleName;

            // Tạo JWT token cục bộ cho ứng dụng
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
            try
            {
                await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(logoutDTO.Token);
                return new ResponseDTO("Logout successful", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error during logout: {ex.Message}", 500, false);
            }
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
