using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Repositories
{
    public class TeamGuardRepository : GenericRepository<TeamGuard>, ITeamGuardRepository
    {
        private readonly SafeguardDbContext _context;

        public TeamGuardRepository(SafeguardDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TeamGuard> AddAsync(TeamGuard entity)
        {
            await _context.TeamGuards.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TeamGuard> RemoveAsync(TeamGuard entity)
        {
            _context.TeamGuards.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<bool> Any(Expression<Func<TeamGuard, bool>> predicate)
        {
            return _context.TeamGuards.AnyAsync(predicate);
        }
    }
}
