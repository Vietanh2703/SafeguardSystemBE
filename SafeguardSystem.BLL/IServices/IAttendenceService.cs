using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IAttendenceService
{
    Task<ResponseDTO> CheckInAsync(Guid attendenceId, AttendenceDTO attendenceDTO);
    Task<ResponseDTO> UpdateAbsentAttendancesAsync();
}