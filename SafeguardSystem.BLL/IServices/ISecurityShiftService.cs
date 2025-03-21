using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface ISecurityshiftService
    {
        Task<ResponseDTO> AssignShiftAsync(Guid locationId, Guid teamId, Guid typeId);
        Task<ResponseDTO> DeleteShiftAsync(Guid shiftId);
    }
}
