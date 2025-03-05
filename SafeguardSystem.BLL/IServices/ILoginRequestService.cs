using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface ILoginRequestService
    {
        Task<ResponseDTO> GetAllRequests(int pageNumber, int pageSize);
        Task<ResponseDTO> ApprovalLoginGoogle(Guid requestId, string status, string reason);
    }
}
