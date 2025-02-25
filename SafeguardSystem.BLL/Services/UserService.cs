using FirebaseAdmin.Auth;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SafeguardSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    IsActive = false,
                    IsEmailConfirmed = false,  // Tùy theo luồng xác nhận email của bạn
                    IsDeleted = false
                    // Các thuộc tính khác nếu cần
                };

                 _unitOfWork.Users.Add(newUser);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("User created successfully", 200, true, newUser);
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDTO($"Error creating user: {errorDetails}", 500, false);
            }
        }
    }
}
