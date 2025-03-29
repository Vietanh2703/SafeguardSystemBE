using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IGuardService
{
    Task<ResponseDTO> GetGuardByIdAsync(Guid guardId);
    Task<ResponseDTO> UpdateGuardAsync(Guid GuardId, SecurityGuardDTO guardDTO);
    Task<ResponseDTO> GetAllGuardAsync();
}