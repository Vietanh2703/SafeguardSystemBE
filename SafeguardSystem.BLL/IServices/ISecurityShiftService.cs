using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface ISecurityshiftService
{
    Task<ResponseDTO> GetShiftsByGuardIdAsync(Guid guardId);
    Task<ResponseDTO> AssignShiftAsync(AssignShiftDTO securityShiftDTO);
    Task<ResponseDTO> DeleteShiftAsync(Guid shiftId);
    Task<ResponseDTO> GetShiftsByShiftDateAsync(DateOnly shiftDate);
    Task<ResponseDTO> ViewAllShiftsAsync();
}