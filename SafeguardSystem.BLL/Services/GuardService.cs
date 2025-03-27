using FirebaseAdmin.Auth;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class GuardService : IGuardService
{
    private readonly IUnitOfWork _unitOfWork;

    public GuardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDTO> GetGuardByIdAsync(Guid guardId)
    {
        var guard = await _unitOfWork.SecurityGuards.GetByIdAsync(guardId);
        if (guard == null) return new ResponseDTO("Guard info not found", 404);

        var guardDTO = new SecurityGuardDTO
        {
            Avatar = guard.User?.Avatar ?? "https://example.com/default-avatar.png", // Avatar mặc định
            IdentityNumber = guard.IdentityNumber,
            Email = guard.User?.Email ?? "N/A",
            FullName = guard.User?.FullName ?? "Unknown",
            PhoneNumber = guard.User?.Phone ?? "N/A",
            BirthDay = guard.User?.BirthDay
        };

        return new ResponseDTO("Guard retrieved successfully", 200, true, guardDTO);
    }

    public async Task<ResponseDTO> UpdateGuardAsync(Guid GuardId, SecurityGuardDTO guardDTO)
    {
        if (guardDTO == null) return new ResponseDTO("Invalid guard info", 400);
        var guard = await _unitOfWork.SecurityGuards.GetByIdAsync(GuardId);
        if (guard == null) return new ResponseDTO("Guard info not found", 404);
        // Kiểm tra user có tồn tại trong Firebase không
        var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(guard.User.UserId);
        if (user == null) return new ResponseDTO("User not found in database", 404);

        // Kiểm tra nếu người dùng muốn đổi mật khẩu
        if (!string.IsNullOrEmpty(guardDTO.Password))
            if (guardDTO.Password != guardDTO.ConfirmPassword)
                return new ResponseDTO("Password and Confirm Password do not match", 400);

        // Cập nhật thông tin trên Firebase
        try
        {
            var firebaseUser = await FirebaseAuth.DefaultInstance.GetUserAsync(guard.User.UserId);
            var userRecordArgs = new UserRecordArgs
            {
                Uid = guard.User.UserId,
                DisplayName = guardDTO.FullName ?? firebaseUser.DisplayName
            };

            // Nếu có mật khẩu mới, cập nhật trên Firebase
            if (!string.IsNullOrEmpty(guardDTO.Password)) userRecordArgs.Password = guardDTO.Password;

            await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);
        }
        catch (FirebaseAuthException ex)
        {
            return new ResponseDTO($"Firebase error: {ex.Message}", 500);
        }

        guard.User.Avatar = guardDTO.Avatar ?? user.Avatar;
        guard.IdentityNumber = guardDTO.IdentityNumber;
        guard.User.Email = guardDTO.Email ?? user.Email;
        guard.User.FullName = guardDTO.FullName ?? user.FullName;
        guard.User.Phone = guardDTO.PhoneNumber ?? user.Phone;
        guard.User.BirthDay = guardDTO.BirthDay != DateOnly.MinValue ? guardDTO.BirthDay : user.BirthDay;

        await _unitOfWork.SaveChangeAsync();

        return new ResponseDTO("Guard updated successfully", 200, true);
    }

    public async Task<ResponseDTO> GetAllGuard()
    {
        var guards = await _unitOfWork.SecurityGuards.GetAllGuardsAsync();
        if (guards == null || !guards.Any()) return new ResponseDTO("Empty guard in list.", 200);
        var guardDTOs = guards.Select(g => new SecurityGuardDTO
        {
            Avatar = g.User?.Avatar ?? "https://example.com/default-avatar.png", // Avatar mặc định
            IdentityNumber = g.IdentityNumber,
            Email = g.User?.Email ?? "N/A",
            FullName = g.User?.FullName ?? "Unknown",
            PhoneNumber = g.User?.Phone ?? "N/A",
            BirthDay = g.User?.BirthDay
        }).ToList();
        return new ResponseDTO("Retrieve guard: ", 200, true, guardDTOs);
    }
}