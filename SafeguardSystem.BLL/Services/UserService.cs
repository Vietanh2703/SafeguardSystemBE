using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUrlHelper Url;

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
                // Check if a user with the given email or username already exists
                var existingUserResponse = await _unitOfWork.Users.GetUserByEmailAsync(createUserDTO.Email);
                if (existingUserResponse != null)
                {
                    return new ResponseDTO("User with this email or username already exists", 400, false);
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
                    // Handle Firebase errors (e.g., email already exists on Firebase)
                    return new ResponseDTO($"Firebase error: {ex.Message}", 500, false);
                }

                var otp = new Random().Next(100000, 999999).ToString();
                var otpExpiry = DateTime.UtcNow.AddMinutes(10); // OTP expires after 10 minutes

                // Create new user record in MySQL
                var newUser = new User
                {
                    UserId = userRecord.Uid,   // UID from Firebase
                    Email = createUserDTO.Email,
                    UserName = createUserDTO.Email,      // Can be changed if there is a separate username
                    FullName = createUserDTO.FullName,
                    Avatar = "https://www.didongmy.com/vnt_upload/news/05_2024/anh-13-meme-dang-yeu-didongmy.jpg",
                    Phone = createUserDTO.Phone,
                    RoleID = createUserDTO.RoleId,
                    ActivationToken = otp,
                    ActivationTokenExpiry = otpExpiry,
                    IsActive = false,
                    IsEmailConfirmed = false,  // Depending on your email confirmation flow
                    IsDeleted = false
                    // Other properties if needed
                };

                _unitOfWork.Users.Add(newUser);
                await _unitOfWork.SaveChangeAsync();
                await SendOtpEmail(newUser.Email, otp, newUser.FullName);
                return new ResponseDTO("User created successfully.", 200, true);
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDTO($"Error creating user: {errorDetails}", 500, false);
            }
        }


        // Gửi email xác thực OTP
        public async Task SendOtpEmail(string Email, string OtpText, string FullName)
        {
            var emailRequest = new EmailRequest();
            emailRequest.Email = Email;
            emailRequest.Subject = "Your OTP Code for Account Activation";
            emailRequest.EmailBody = _emailService.GenerateEmailBody(FullName, OtpText);
            await _emailService.SendActivationEmailAsync(emailRequest);
        }

        // Xác thực OTP và kích hoạt tài khoản
        public async Task<ResponseDTO> VerifyOtpAsync(OtpDTO OtpDTO)
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

        public async Task<ResponseDTO> GetAllRolesAsync()
        {
            var roles = await _unitOfWork.Roles.GetAll().ToListAsync();
            var roleDTOs = roles.Select(r => new RoleDTO
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            }).ToList();

            return new ResponseDTO("Roles list:", 200, true, roleDTOs);
        }
    }
}
