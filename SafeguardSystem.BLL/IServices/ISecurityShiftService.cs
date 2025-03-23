using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface ISecurityshiftService
{
    Task<ResponseDTO> GetShiftsByGuardIdAsync(Guid guardId);
    Task<ResponseDTO> AssignShiftAsync(SecurityShiftDTO securityShiftDTO);
    Task<ResponseDTO> DeleteShiftAsync(Guid shiftId);
}