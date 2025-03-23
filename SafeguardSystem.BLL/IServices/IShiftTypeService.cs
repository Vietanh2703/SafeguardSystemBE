using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IShiftTypeService
{
    Task<ResponseDTO> GetAllShiftTypesAsync();
    Task<ResponseDTO> CreateShiftTypeAsync(ShiftTypeDTO shiftTypeDTO);
    Task<ResponseDTO> UpdateShiftTypeAsync(Guid TypeId, ShiftTypeDTO shiftTypeDTO);
    Task<ResponseDTO> DeleteShiftTypeAsync(Guid TypeId);
}