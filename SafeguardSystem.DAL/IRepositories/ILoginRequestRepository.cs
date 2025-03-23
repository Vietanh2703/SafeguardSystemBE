using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;

namespace SafeguardSystem.DAL.IRepositories;

public interface ILoginRequestRepository : IGenericRepository<LoginRequest>
{
    Task<int> CountPendingOrRejectRequestAsync(string id);
    Task<PaginatedList<LoginRequest>> GetAllRequestsPagingAsync(int pageNumber, int pageSize);
}