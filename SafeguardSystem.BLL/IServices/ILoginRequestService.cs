using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface ILoginRequestService
{
    Task<ResponseDTO> GetAllRequests(int pageNumber, int pageSize);
    Task<ResponseDTO> ApprovalLoginGoogle(Guid requestId, string status, string reason);
}