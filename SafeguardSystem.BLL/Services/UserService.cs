using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
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

        public async Task SendOtpEmail(string Email, string OtpText, string FullName)
        {
            var emailRequest = new EmailRequest();
            emailRequest.Email = Email;
            emailRequest.Subject = "Your OTP Code for Account Activation";
            emailRequest.EmailBody = _emailService.GenerateEmailBody(FullName, OtpText);
            await _emailService.SendActivationEmailAsync(emailRequest);
        }

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
    }
}
