using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface IShiftTypeService
    {
        Task<ResponseDTO> GetAllShiftTypesAsync();
        Task<ResponseDTO> CreateShiftTypeAsync(ShiftTypeDTO shiftTypeDTO);
        Task<ResponseDTO> UpdateShiftTypeAsync(Guid TypeId, ShiftTypeDTO shiftTypeDTO);
        Task<ResponseDTO> DeleteShiftTypeAsync(Guid TypeId);
    }
}
