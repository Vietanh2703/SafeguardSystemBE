using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface ILoginRequestRepository : IGenericRepository<LoginRequest>
    {
        Task<int> CountPendingOrRejectRequestAsync(string id);
        Task<PaginatedList<LoginRequest>> GetAllRequestsPagingAsync(int pageNumber, int pageSize);
    }
}
