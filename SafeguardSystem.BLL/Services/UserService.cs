using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public UserService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        // Tạo người dùng mới trên Firebase và MySQL
        public async Task<ResponseDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            try
            {
                // Check if a user with the given email already exists
                var existingUserResponse = await _unitOfWork.Users.GetUserByEmailAsync(createUserDTO.Email);
                if (existingUserResponse != null)
                {
                    return new ResponseDTO("User with this email already exists", 400, false);
                }

                // Check if the provided RoleID exists
                var role = await _unitOfWork.Roles.GetByGuidAsync(createUserDTO.RoleId);
                if (role == null)
                {
                    return new ResponseDTO("Invalid RoleID provided", 400, false);
                }

                var userRecordArgs = new UserRecordArgs
                {
                    Email = createUserDTO.Email,
                    Password = createUserDTO.PassWord,
                    DisplayName = createUserDTO.FullName,
                };

                // Create user on Firebase
                UserRecord userRecord;
                try
                {
                    userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);
                }
                catch (FirebaseAuthException ex)
                {
                    return new ResponseDTO($"Firebase error: {ex.Message}", 500, false);
                }

                // Create new user in MySQL
                var newUser = new User
                {
                    UserId = userRecord.Uid,
                    Email = createUserDTO.Email,
                    UserName = createUserDTO.Email,
                    FullName = createUserDTO.FullName,
                    Avatar = "https://www.didongmy.com/vnt_upload/news/05_2024/anh-13-meme-dang-yeu-didongmy.jpg",
                    Phone = createUserDTO.Phone,
                    RoleID = createUserDTO.RoleId,
                    ActivationToken = "N/A",
                    ActivationTokenExpiry = null,
                    IsActive = false,
                    IsEmailConfirmed = false,
                    IsLocked = false,
                    IsDeleted = false
                };

                _unitOfWork.Users.Add(newUser);
                await _unitOfWork.SaveChangeAsync();

                //Tạo SecurityGuard nếu Role là Security Guard
                var securityGuardRoleId = await _unitOfWork.Roles.GetSecurityGuardRoleIdAsync();
                if (createUserDTO.RoleId == securityGuardRoleId)
                {
                    // Ensure a valid TeamId is provided or handle the case where no valid TeamId is available
                    var newGuard = new SecurityGuard
                    {
                        GuardId = Guid.NewGuid(),
                        UserId = newUser.UserId,
                        Status = "PENDING", // hoặc dùng Enum
                        Latitude = 0,
                        Longitude = 0,
                        IdentityNumber = "",
                        StartDate = DateTime.UtcNow
                    };

                    _unitOfWork.SecurityGuards.Add(newGuard);
                    await _unitOfWork.SaveChangeAsync();
                }

                await SendWelcomeEmail(newUser.FullName, newUser.Email, userRecordArgs.Password);
                return new ResponseDTO("User created successfully.", 200, true);
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDTO($"Error creating user: {errorDetails}", 500, false);
            }
        }

        //Gửi email chào mừng để khách hàng có được email và password đăng nhập
        public async Task SendWelcomeEmail(string FullName, string Email, string Password)
        {
            var emailRequest = new EmailRequest();
            emailRequest.Email = Email;
            emailRequest.Subject = "[NO-REPLY]Welcome to Safeguard System";
            emailRequest.EmailBody = _emailService.GenerateWelcomeEmailBody(FullName, Email, Password);
            await _emailService.SendEmailAsync(emailRequest);
        }

        // Gửi email xác thực OTP
        public async Task SendOtpEmail(string Email, string OtpText, string FullName)
        {
            var emailRequest = new EmailRequest();
            emailRequest.Email = Email;
            emailRequest.Subject = "Your OTP Code for Account Activation";
            emailRequest.EmailBody = _emailService.GenerateOtpEmailBody(FullName, OtpText);
            await _emailService.SendEmailAsync(emailRequest);
        }

        //Làm mới OTP và gửi email
        public async Task<ResponseDTO> RefreshOtpAsync(string email)
        {
            try
            {
                // Check if the user exists and is not email confirmed
                var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return new ResponseDTO("User not found.", 404, false);
                }

                if (user.IsEmailConfirmed)
                {
                    return new ResponseDTO("Email is already confirmed.", 400, false);
                }

                // Generate new OTP
                var otp = new Random().Next(100000, 999999).ToString();
                user.ActivationToken = otp;
                user.ActivationTokenExpiry = DateTime.UtcNow.AddMinutes(10);

                await _unitOfWork.SaveChangeAsync();

                // Send OTP email
                await SendOtpEmail(user.Email, otp, user.FullName);

                return new ResponseDTO("OTP has been refreshed and sent to your email.", 200, true);
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDTO($"Error refreshing OTP: {errorDetails}", 500, false);
            }
        }

        // Xác thực OTP và kích hoạt tài khoản
        public async Task<ResponseDTO> VerifyOtpAsync(string Email, OtpDTO OtpDTO)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByEmailAsync(OtpDTO.Email);
                if (user == null)
                {
                    return new ResponseDTO("User not found.", 404, false);
                }

                if (user.ActivationToken != OtpDTO.Otp)
                {
                    return new ResponseDTO("Invalid OTP.", 400, false);
                }

                if (user.ActivationTokenExpiry < DateTime.UtcNow)
                {
                    return new ResponseDTO("OTP has expired.", 400, false);
                }

                // Cập nhật tài khoản khi OTP hợp lệ
                user.IsActive = true;
                user.IsEmailConfirmed = true;
                user.ActivationToken = null;  // Xóa OTP
                user.ActivationTokenExpiry = null;  // Xóa thời gian hết hạn OTP
                await _unitOfWork.SaveChangeAsync();

                // Gửi email thông báo xác thực thành công
                var emailRequest = new EmailRequest();
                emailRequest.Email = Email;
                emailRequest.Subject = "Your OTP Code for Account Activation Successfully";
                emailRequest.EmailBody = _emailService.GenerateActivationSuccessEmailBody(user.FullName);
                await _emailService.SendEmailAsync(emailRequest);

                return new ResponseDTO("Email verified successfully.", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error verifying OTP: {ex.Message}", 500, false);
            }
        }

        // Lấy thông tin người dùng và phân trang
        public async Task<ResponseDTO> GetAllUsersAsync(int pageIndex, int pageSize)
        {
            var paginatedUsers = await _unitOfWork.Users.GetAllUsersWithPagingAsync(pageIndex, pageSize);
            if (paginatedUsers == null || !paginatedUsers.Any())
            {
                return new ResponseDTO("No users found in list.", 200, false);
            }

            var userDTOs = paginatedUsers
                .Where(u => !u.IsDeleted)
                .Select(u => new ViewUserListDTO
                {
                    Email = u.Email,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Avatar = u.Avatar
                }).ToList();

            return new ResponseDTO("User list:", 200, true, new PaginatedList<ViewUserListDTO>(userDTOs, paginatedUsers.Count, pageIndex, pageSize));
        }

        //Lấy thông tin người dùng không phân trang
        public async Task<ResponseDTO> GetAllUsersAsync()
        {
            try
            {
                var allUsers = await _unitOfWork.Users.GetAllUsersAsync();
                if (allUsers == null || !allUsers.Any())
                {
                    return new ResponseDTO("No users found in the system.", 200, false);
                }

                var userDTOs = allUsers
                    .Where(u => !u.IsDeleted)
                    .Select(u => new ViewUserListDTO
                    {
                        Email = u.Email,
                        FullName = u.FullName,
                        Phone = u.Phone,
                        Avatar = u.Avatar
                    }).ToList();

                return new ResponseDTO("Users retrieved successfully.", 200, true, userDTOs);
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDTO($"Error retrieving users: {errorDetails}", 500, false);
            }
        }

        // Lấy thông tin người dùng theo vai trò
        public async Task<ResponseDTO> GetUsersByRoleAsync(string roleName)
        {
            try
            {
                // Trả ra tên role
                var role = await _unitOfWork.Roles.GetRoleIdByNameAsync(roleName);
                if (role == null)
                {
                    return new ResponseDTO("Role not found", 404, false);
                }

                // trả ra tất cả role trừ "Admin"
                var users = await _unitOfWork.Users.GetAll()
                    .Where(u => u.RoleID == role.RoleId && !u.Role.RoleName.ToLower().Equals("admin"))
                    .ToListAsync();

                if (users == null || !users.Any())
                {
                    return new ResponseDTO("No users found for the specified role", 200, false);
                }

                var userDTOs = users
                    .Where(u => !u.IsDeleted)
                    .Select(u => new ViewUserListDTO
                    {
                        Email = u.Email,
                        FullName = u.FullName,
                        Phone = u.Phone,
                        Avatar = u.Avatar
                    }).ToList();

                return new ResponseDTO("Users retrieved successfully.", 200, true, userDTOs);
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDTO($"Error retrieving users: {errorDetails}", 500, false);
            }
        }

        // Xóa người dùng khỏi cả MySQL và Firebase
        public async Task<ResponseDTO> DeleteUserAsync(string userId)
        {
            try
            {
                // Tìm người dùng trong MySQL bằng UserId
                var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
                if (user == null)
                {
                    return new ResponseDTO("User not found", 404, false);
                }

                // Xóa người dùng khỏi Firebase Authentication
                try
                {
                    await FirebaseAuth.DefaultInstance.DeleteUserAsync(userId);
                }
                catch (FirebaseAuthException ex)
                {
                    return new ResponseDTO($"Firebase error: {ex.Message}", 500, false);
                }

                // Cập nhật trạng thái soft delete trong MySQL
                user.IsDeleted = true;
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("User has been deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error deleting user: {ex.Message}", 500, false);
            }
        }

        // Cập nhật mật khẩu người dùng lần đầu đăng nhập
        public async Task<ResponseDTO> UpdatePasswordAsync(string email, UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {
                // Check if the user exists and is not email confirmed
                var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return new ResponseDTO("User not found.", 404, false);
                }

                if (user.IsEmailConfirmed)
                {
                    return new ResponseDTO("Email is already confirmed.", 400, false);
                }

                // Validate password and confirm password
                if (string.IsNullOrEmpty(updatePasswordDTO.Password) || string.IsNullOrEmpty(updatePasswordDTO.ConfirmPassword))
                {
                    return new ResponseDTO("Password and Confirm Password are required.", 400, false);
                }

                if (updatePasswordDTO.Password != updatePasswordDTO.ConfirmPassword)
                {
                    return new ResponseDTO("Password and Confirm Password do not match.", 400, false);
                }

                // Check password conditions (e.g., length, complexity)
                if (updatePasswordDTO.Password.Length < 6)
                {
                    return new ResponseDTO("Password must be at least 6 characters long.", 400, false);
                }

                // Generate OTP
                var otp = new Random().Next(100000, 999999).ToString();
                user.ActivationToken = otp;
                user.ActivationTokenExpiry = DateTime.UtcNow.AddMinutes(10);

                // Update password on Firebase
                try
                {
                    var userRecordArgs = new UserRecordArgs
                    {
                        Uid = user.UserId,
                        Password = updatePasswordDTO.Password
                    };
                    await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);
                }
                catch (FirebaseAuthException ex)
                {
                    return new ResponseDTO($"Firebase error: {ex.Message}", 500, false);
                }

                await _unitOfWork.SaveChangeAsync();

                // Send OTP email
                await SendOtpEmail(user.Email, otp, user.FullName);

                return new ResponseDTO("Password updated successfully. Please check your email for the OTP to confirm your email.", 200, true);
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDTO($"Error updating password: {errorDetails}", 500, false);
            }
        }

        // Cập nhật thông tin người dùng
        public async Task<ResponseDTO> UpdateUserAsync(string userId, UpdateUserDTO updateUserDTO)
        {
            try
            {
                // Kiểm tra user có tồn tại trong MySQL không
                var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
                if (user == null)
                {
                    return new ResponseDTO("User not found in database", 404, false);
                }

                // Kiểm tra nếu người dùng muốn đổi mật khẩu
                if (!string.IsNullOrEmpty(updateUserDTO.Password))
                {
                    if (updateUserDTO.Password != updateUserDTO.ConfirmPassword)
                    {
                        return new ResponseDTO("Password and Confirm Password do not match", 400, false);
                    }
                }

                // Cập nhật thông tin trên Firebase
                try
                {
                    var firebaseUser = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);
                    var userRecordArgs = new UserRecordArgs
                    {
                        Uid = userId,
                        DisplayName = updateUserDTO.FullName ?? firebaseUser.DisplayName,
                    };

                    // Nếu có mật khẩu mới, cập nhật trên Firebase
                    if (!string.IsNullOrEmpty(updateUserDTO.Password))
                    {
                        userRecordArgs.Password = updateUserDTO.Password;
                    }

                    await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);
                }
                catch (FirebaseAuthException ex)
                {
                    return new ResponseDTO($"Firebase error: {ex.Message}", 500, false);
                }

                // Cập nhật thông tin trong MySQL
                user.UserName = updateUserDTO.UserName ?? user.UserName;
                user.FullName = updateUserDTO.FullName ?? user.FullName;
                user.Phone = updateUserDTO.Phone ?? user.Phone;
                user.Avatar = updateUserDTO.Avatar ?? user.Avatar;
                user.BirthDay = updateUserDTO.Birthday != DateTime.MinValue ? updateUserDTO.Birthday : user.BirthDay;
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("User profile updated successfully", 200, true, user);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error updating user profile: {ex.Message}", 500, false);
            }
        }

        // Lấy danh sách các Role
        public async Task<ResponseDTO> GetAllRolesAsync()
        {
            var roles = await _unitOfWork.Roles.GetAll()
                .Where(r => !r.RoleName.ToLower().Equals("Admin"))
                .ToListAsync();

            var roleDTOs = roles.Select(r => new RoleDTO
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            }).ToList();

            return new ResponseDTO("Roles list:", 200, true, roleDTOs);
        }

        //Lấy thông tin người dùng theo UserId
        public async Task<ResponseDTO> GetUserByUserIdAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
            if (user == null)
            {
                return new ResponseDTO("User not found", 404, false);
            }

            // Retrieve the role information
            var role = await _unitOfWork.Roles.GetByGuidAsync(user.RoleID);
            if (role == null)
            {
                return new ResponseDTO("Role not found", 404, false);
            }

            // Create the UserDTO
            var userDTO = new UserDTO
            {
                FullName = user.FullName,
                Phone = user.Phone,
                Avatar = user.Avatar,
                BirthDay = user.BirthDay,
                UserName = user.UserName,
            };

            // If the role name is "Security Guard", include the IdentityNumber
            if (role.RoleName.Equals("Security Guard", StringComparison.OrdinalIgnoreCase))
            {
                var securityGuard = await _unitOfWork.SecurityGuards.FirstOrDefaultAsync(g => g.UserId == userId);
                if (securityGuard != null)
                {
                    userDTO.IdentityNumber = securityGuard.IdentityNumber;
                }
            }

            return new ResponseDTO("User found", 200, true, userDTO);
        }
        
    }
}
