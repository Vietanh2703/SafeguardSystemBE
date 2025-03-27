using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class UserService : IUserService
{
    private const long MaxFileSize = 20 * 1024 * 1024; //Max file size is 20MB

    private static readonly List<string> AllowedFileTypes = new()
    {
        // Image file types
        "image/jpeg", "image/png"
    };

    private readonly IAWSS3Service _awsS3Service;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork, IEmailService emailService, IAWSS3Service aWSS3Service)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _awsS3Service = aWSS3Service;
    }

    // Tạo người dùng mới trên Firebase và MySQL
    public async Task<ResponseDTO> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        try
        {
            // Check if a user with the given email already exists
            var existingUserResponse = await _unitOfWork.Users.GetUserByEmailAsync(createUserDTO.Email);
            if (existingUserResponse != null) return new ResponseDTO("User with this email already exists", 400);

            // Check if the provided RoleID exists
            var role = await _unitOfWork.Roles.GetByGuidAsync(createUserDTO.RoleId);
            if (role == null) return new ResponseDTO("Invalid RoleID provided", 400);

            var userRecordArgs = new UserRecordArgs
            {
                Email = createUserDTO.Email,
                Password = createUserDTO.PassWord,
                DisplayName = createUserDTO.FullName
            };

            // Create user on Firebase
            UserRecord userRecord;
            try
            {
                userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);
            }
            catch (FirebaseAuthException ex)
            {
                return new ResponseDTO($"Firebase error: {ex.Message}", 500);
            }

            // Create new user in MySQL
            var newUser = new User
            {
                UserId = userRecord.Uid,
                Email = createUserDTO.Email,
                UserName = createUserDTO.Email,
                FullName = createUserDTO.FullName,
                Avatar = "58sErANL7bbv096ghTnNN3qIiqX2_cooper.jpg",
                Phone = createUserDTO.Phone,
                RoleID = createUserDTO.RoleId,
                Address = "N/A",
                Gender = "N/A",
                WorkingContract = "N/A",
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
            return new ResponseDTO($"Error creating user: {errorDetails}", 500);
        }
    }

    //Làm mới OTP và gửi email
    public async Task<ResponseDTO> RefreshOtpAsync(string email)
    {
        try
        {
            // Check if the user exists and is not email confirmed
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (user == null) return new ResponseDTO("User not found.", 404);

            if (user.IsEmailConfirmed) return new ResponseDTO("Email is already confirmed.", 400);

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
            return new ResponseDTO($"Error refreshing OTP: {errorDetails}", 500);
        }
    }

    // Xác thực OTP và kích hoạt tài khoản
    public async Task<ResponseDTO> VerifyOtpAsync(string Email, OtpDTO OtpDTO)
    {
        try
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(Email);
            if (user == null) return new ResponseDTO("User not found.", 404);

            if (user.ActivationToken != OtpDTO.Otp) return new ResponseDTO("Invalid OTP.", 400);

            if (user.ActivationTokenExpiry < DateTime.UtcNow) return new ResponseDTO("OTP has expired.", 400);

            // Cập nhật tài khoản khi OTP hợp lệ
            user.IsActive = true;
            user.IsEmailConfirmed = true;
            user.ActivationToken = null; // Xóa OTP
            user.ActivationTokenExpiry = null; // Xóa thời gian hết hạn OTP
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
            return new ResponseDTO($"Error verifying OTP: {ex.Message}", 500);
        }
    }

    // Lấy thông tin người dùng và phân trang
    public async Task<ResponseDTO> GetAllUsersAsync(int pageIndex, int pageSize)
    {
        var paginatedUsers = await _unitOfWork.Users.GetAllUsersWithPagingAsync(pageIndex, pageSize);

        if (paginatedUsers == null || !paginatedUsers.Any())
            return new ResponseDTO(
                "No users found in list.",
                200,
                false,
                new
                {
                    users = new List<ViewUserListDTO>(),
                    totalPages = 0,
                    totalItems = 0
                }
            );

        var userDTOs = paginatedUsers
            .Where(u => !u.IsDeleted)
            .Select(u => new ViewUserListDTO
            {
                UserId = u.UserId,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                Avatar = u.Avatar
            }).ToList();

        var totalUsers = await _unitOfWork.Users.GetTotalUserCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

        return new ResponseDTO(
            "User list.",
            200,
            true,
            new
            {
                users = userDTOs,
                totalPages,
                totalUsers
            }
        );
    }

    //Lấy thông tin người dùng không phân trang
    public async Task<ResponseDTO> GetAllUsersAsync()
    {
        try
        {
            var allUsers = await _unitOfWork.Users.GetAllUsersAsync();
            if (allUsers == null || !allUsers.Any()) return new ResponseDTO("No users found in the system.", 200);

            var userDTOs = allUsers
                .Where(u => !u.IsDeleted)
                .Select(u => new ViewUserListDTO
                {
                    UserId = u.UserId,
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
            return new ResponseDTO($"Error retrieving users: {errorDetails}", 500);
        }
    }

    // Lấy thông tin người dùng theo vai trò
    public async Task<ResponseDTO> GetUsersByRoleAsync(string roleName)
    {
        try
        {
            // Trả ra tên role
            var role = await _unitOfWork.Roles.GetRoleIdByNameAsync(roleName);
            if (role == null) return new ResponseDTO("Role not found", 404);

            // trả ra tất cả role trừ "Admin"
            var users = await _unitOfWork.Users.GetAll()
                .Where(u => u.RoleID == role.RoleId && !u.Role.RoleName.ToLower().Equals("admin"))
                .ToListAsync();

            if (users == null || !users.Any()) return new ResponseDTO("No users found for the specified role", 200);

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
            return new ResponseDTO($"Error retrieving users: {errorDetails}", 500);
        }
    }

    // Xóa người dùng khỏi cả MySQL và Firebase
    public async Task<ResponseDTO> DeleteUserAsync(string userId)
    {
        try
        {
            // Tìm người dùng trong MySQL bằng UserId
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
            if (user == null) return new ResponseDTO("User not found", 404);

            // Xóa người dùng khỏi Firebase Authentication
            try
            {
                await FirebaseAuth.DefaultInstance.DeleteUserAsync(userId);
            }
            catch (FirebaseAuthException ex)
            {
                return new ResponseDTO($"Firebase error: {ex.Message}", 500);
            }

            // Cập nhật trạng thái soft delete trong MySQL
            user.IsDeleted = true;
            user.IsActive = false;
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("User has been deleted successfully", 200, true);
        }
        catch (Exception ex)
        {
            return new ResponseDTO($"Error deleting user: {ex.Message}", 500);
        }
    }

    // Cập nhật mật khẩu người dùng lần đầu đăng nhập
    public async Task<ResponseDTO> UpdatePasswordAsync(string email, UpdatePasswordDTO updatePasswordDTO)
    {
        try
        {
            // Check if the user exists and is not email confirmed
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (user == null) return new ResponseDTO("User not found.", 404);

            if (user.IsEmailConfirmed) return new ResponseDTO("Email is already confirmed.", 400);

            // Validate password and confirm password
            if (string.IsNullOrEmpty(updatePasswordDTO.Password) ||
                string.IsNullOrEmpty(updatePasswordDTO.ConfirmPassword))
                return new ResponseDTO("Password and Confirm Password are required.", 400);

            if (updatePasswordDTO.Password != updatePasswordDTO.ConfirmPassword)
                return new ResponseDTO("Password and Confirm Password do not match.", 400);

            // Check password conditions (e.g., length, complexity)
            if (updatePasswordDTO.Password.Length < 6)
                return new ResponseDTO("Password must be at least 6 characters long.", 400);

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
                return new ResponseDTO($"Firebase error: {ex.Message}", 500);
            }

            await _unitOfWork.SaveChangeAsync();

            // Send OTP email
            await SendOtpEmail(user.Email, otp, user.FullName);

            return new ResponseDTO(
                "Password updated successfully. Please check your email for the OTP to confirm your email.", 200,
                true);
        }
        catch (Exception ex)
        {
            var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new ResponseDTO($"Error updating password: {errorDetails}", 500);
        }
    }

    // Cập nhật thông tin người dùng
    public async Task<ResponseDTO> UpdateUserAsync(string userId, UpdateUserDTO updateUserDTO)
    {
        try
        {
            // Kiểm tra user có tồn tại trong MySQL không
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
            if (user == null) return new ResponseDTO("User not found in database", 404);

            // Cập nhật thông tin trên Firebase
            try
            {
                var firebaseUser = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);
                var userRecordArgs = new UserRecordArgs
                {
                    Uid = userId,
                    DisplayName = updateUserDTO.FullName ?? firebaseUser.DisplayName
                };


                await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);
            }
            catch (FirebaseAuthException ex)
            {
                return new ResponseDTO($"Firebase error: {ex.Message}", 500);
            }

            //Xác thực file ảnh có hợp lệ không
            //if (updateUserDTO.Avatar != null)
            //{
            //    var fileVerificationResult = FileVerification(updateUserDTO.Avatar);
            //    if (!fileVerificationResult.IsSuccess)
            //    {
            //        return fileVerificationResult;
            //    }
            //}

            // Upload ảnh lên AWS S3
            //string ImageUrl = null;
            //if (updateUserDTO.Avatar != null)
            //{
            //    var uploadResult = await _awsS3Service.DefaultUploadFileAsync(updateUserDTO.Avatar, userId);
            //    if (!uploadResult.IsSuccess)
            //    {
            //        return new ResponseDTO($"Failed to upload image: {uploadResult.Message}", 500, false);
            //    }
            //    ImageUrl = uploadResult.Result.ToString();
            //}

            // Cập nhật thông tin trong MySQL
            user.WorkingContract = updateUserDTO.WorkingContract;
            user.Address = updateUserDTO.Address;
            user.Phone = updateUserDTO.Phone;
            user.BirthDay = updateUserDTO.Birthday != DateOnly.MinValue ? updateUserDTO.Birthday : user.BirthDay;
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("User profile updated successfully", 200, true, user);
        }
        catch (Exception ex)
        {
            return new ResponseDTO($"Error updating user profile: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDTO> UpdateAvatarAsync(string userId, AvatarDTO avatarDTO)
    {
        try
        {
            // Validate the file
            var fileVerificationResult = FileVerification(avatarDTO.AvatarFile);
            if (!fileVerificationResult.IsSuccess)
            {
                return fileVerificationResult;
            }

            // Find the user in MySQL by UserId
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
            if (user == null) return new ResponseDTO("User not found", 404);

            // Upload the avatar to AWS S3
            var uploadResult = await _awsS3Service.DefaultUploadFileAsync(avatarDTO.AvatarFile, userId);
            if (!uploadResult.IsSuccess)
            {
                return new ResponseDTO($"Failed to upload image: {uploadResult.Message}", 500, false);
            }

            // Update the user's avatar URL in MySQL
            user.Avatar = uploadResult.Result.ToString();
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Avatar updated successfully", 200, true, user.Avatar);
        }
        catch (Exception ex)
        {
            var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new ResponseDTO($"Error updating avatar: {errorDetails}", 500);
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
        if (user == null) return new ResponseDTO("User not found", 404);

        // Retrieve the role information
        var role = await _unitOfWork.Roles.GetByGuidAsync(user.RoleID);
        if (role == null) return new ResponseDTO("Role not found", 404);

        // Get the pre-signed URL for the avatar
        string avatarUrl = null;
        if (!string.IsNullOrEmpty(user.Avatar))
        {
            var avatarUrlResponse = await _awsS3Service.GetPreSignedURLAsync(user.Avatar);
            if (!avatarUrlResponse.IsSuccess)
                return new ResponseDTO($"Error retrieving avatar URL: {avatarUrlResponse.Message}", 500);
            avatarUrl = avatarUrlResponse.Result.ToString();
        }

        // Create the UserDTO
        var userDTO = new UserDTO
        {
            UserName = user.UserName,
            FullName = user.FullName,
            Avatar = avatarUrl,
            Address = user.Address,
            Gender = user.Gender,
            WorkingContract = user.WorkingContract,
            Phone = user.Phone,
            BirthDay = user.BirthDay
        };

        // If the role name is "Security Guard", include the IdentityNumber
        if (role.RoleName.Equals("Security Guard", StringComparison.OrdinalIgnoreCase))
        {
            var securityGuard = await _unitOfWork.SecurityGuards.FirstOrDefaultAsync(g => g.UserId == userId);
            if (securityGuard != null) userDTO.IdentityNumber = securityGuard.IdentityNumber;
        }

        return new ResponseDTO("User found", 200, true, userDTO);
    }


    //Chặn người dùng
    public async Task<ResponseDTO> BanUserAsync(string userId)
    {
        try
        {
            // Find the user in MySQL by UserId
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
            if (user == null) return new ResponseDTO("User not found", 404);

            // Check if the user is already banned
            if (user.IsLocked) return new ResponseDTO("User is already banned", 400);

            // Update the user's status to banned
            user.IsLocked = true;
            user.IsActive = false;
            await _unitOfWork.SaveChangeAsync();
            await SendBanEmail(user.Email, user.FullName);

            return new ResponseDTO("User has been banned successfully", 200, true);
        }
        catch (Exception ex)
        {
            var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new ResponseDTO($"Error banning user: {errorDetails}", 500);
        }
    }

    //Mở chặn người dùng
    public async Task<ResponseDTO> UnbanUserAsync(string userId)
    {
        try
        {
            // Find the user in MySQL by UserId
            var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
            if (user == null) return new ResponseDTO("User not found", 404);

            // Check if the user is not banned
            if (!user.IsLocked) return new ResponseDTO("User is not banned, cannot perform unban operation", 400);

            // Update the user's status to active
            user.IsLocked = false;
            user.IsActive = true;
            await _unitOfWork.SaveChangeAsync();
            await SendUnbanEmail(user.Email, user.FullName);

            return new ResponseDTO("User has been unbanned successfully", 200, true);
        }
        catch (Exception ex)
        {
            var errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new ResponseDTO($"Error unbanning user: {errorDetails}", 500);
        }
    }

    //Gửi email chào mừng để khách hàng có được email và password đăng nhập
    public async Task SendWelcomeEmail(string FullName, string Email, string Password)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY] Welcome to Safeguard System";
        emailRequest.EmailBody = _emailService.GenerateWelcomeEmailBody(FullName, Email, Password);
        await _emailService.SendEmailAsync(emailRequest);
    }

    // Gửi email xác thực OTP
    public async Task SendOtpEmail(string Email, string OtpText, string FullName)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY] Your OTP Code for Account Activation";
        emailRequest.EmailBody = _emailService.GenerateOtpEmailBody(FullName, OtpText);
        await _emailService.SendEmailAsync(emailRequest);
    }

    public async Task SendBanEmail(string Email, string FullName)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY] Your Account has been Banned";
        emailRequest.EmailBody = _emailService.GenerateBanUserEmailBody(FullName);
        await _emailService.SendEmailAsync(emailRequest);
    }

    public async Task SendUnbanEmail(string Email, string FullName)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY] Your Account has been Unbanned";
        emailRequest.EmailBody = _emailService.GenerateUnbanUserEmailBody(FullName);
        await _emailService.SendEmailAsync(emailRequest);
    }

    private ResponseDTO FileVerification(IFormFile file)
    {
        var result = new ResponseDTO("StepAttachment not found", 404);

        if (file.Length > MaxFileSize)
        {
            result.StatusCode = 413;
            result.Message = $"File size exceeds the maximum allowed size of {MaxFileSize / (1024 * 1024)} MB.";
            result.IsSuccess = false;
            return result;
        }

        if (!AllowedFileTypes.Contains(file.ContentType))
        {
            result.StatusCode = 415;
            result.Message = "File type is not allowed. Only image and video files are allowed.";
            result.IsSuccess = false;
            return result;
        }

        return new ResponseDTO("File verification successful", 200, true);
    }
}