using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface IGuardService
    {
        Task<ResponseDTO> GetGuardByIdAsync(Guid guardId);
        Task<ResponseDTO> UpdateGuardAsync(Guid GuardId,SecurityGuardDTO guardDTO);
    }
}
