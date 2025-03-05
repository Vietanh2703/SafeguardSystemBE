using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories
{
    public class LoginRequestRepository : GenericRepository<LoginRequest>,ILoginRequestRepository
    {
        private readonly SafeguardDbContext _context;

        public LoginRequestRepository(SafeguardDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CountPendingOrRejectRequestAsync(string userId)
        {
            return await _context.loginRequests.CountAsync(x => x.UserId == userId && (x.Status == "PENDING" || x.Status == "REJECTED"));
        }

        public async Task<PaginatedList<LoginRequest>> GetAllRequestsPagingAsync(int pageNumber, int pageSize)
        {
            var query = _context.loginRequests.AsQueryable();
            return await PaginatedList<LoginRequest>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
