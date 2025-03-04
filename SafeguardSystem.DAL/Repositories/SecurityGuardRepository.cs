using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories
{
    class SecurityGuardRepository : GenericRepository<SecurityGuard>, ISecurityGuardRepository
    {
        private readonly SafeguardDbContext _context;

        public SecurityGuardRepository(SafeguardDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SecurityGuard>> GetAllGuardsAsync()
        {
           return await _context.SecurityGuards
                .Include(g => g.User)
                .ToListAsync();
        }

        public async Task<SecurityGuard?> GetByIdAsync(Guid guardId)
        {
            return await _context.SecurityGuards
                .Include(g => g.User)
                .SingleOrDefaultAsync(g => g.GuardId == guardId);
        }
    }
}
