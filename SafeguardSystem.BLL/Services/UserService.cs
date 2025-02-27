using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
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


                var userRecordArgs = new UserRecordArgs
                {
                    Email = createUserDTO.Email,
                    Password = createUserDTO.PassWord,
                    DisplayName = createUserDTO.FullName,
                };
                // Tạo user trên Firebase
                UserRecord userRecord;
                try
                {
                    userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);
                }
                catch (FirebaseAuthException ex)
                {
                    // Xử lý lỗi của Firebase (ví dụ email đã tồn tại trên Firebase)
                    return new ResponseDTO($"Firebase error: {ex.Message}", 500, false);
                }

                var otp = new Random().Next(100000, 999999).ToString();
                var otpExpiry = DateTime.UtcNow.AddMinutes(10); // OTP hết hạn sau 10 phút

                // Tạo record người dùng mới trong MySQL
                var newUser = new User
                {
                    UserId = userRecord.Uid,   // UID từ Firebase
                    Email = createUserDTO.Email,
                    UserName = createUserDTO.Email,      // Có thể thay đổi nếu có username riêng
                    FullName = createUserDTO.FullName,
                    Avatar = "https://www.didongmy.com/vnt_upload/news/05_2024/anh-13-meme-dang-yeu-didongmy.jpg",
                    Phone = createUserDTO.Phone,
                    RoleID = Guid.Parse("be19e4b3-6664-4afd-9ebb-98e0a073edc9"),
                    ActivationToken = otp,
                    ActivationTokenExpiry = otpExpiry,
                    IsActive = false,
                    IsEmailConfirmed = false,  // Tùy theo luồng xác nhận email của bạn
                    IsDeleted = false
                    // Các thuộc tính khác nếu cần
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

                // Xóa người dùng khỏi MySQL
                _unitOfWork.Users.Delete(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("User deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error deleting user: {ex.Message}", 500, false);
            }
        }

    }
}
