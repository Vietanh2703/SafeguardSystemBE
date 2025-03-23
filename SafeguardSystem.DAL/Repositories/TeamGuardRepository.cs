using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

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
    
    public async Task<IEnumerable<TeamGuard>> GetAllAsync(Expression<Func<TeamGuard, bool>> predicate)
    {
        return await _context.Set<TeamGuard>().Where(predicate).ToListAsync();
    }
    
    public async Task<TeamGuard?> GetAsync(Expression<Func<TeamGuard, bool>> predicate)
    {
        return await _context.Set<TeamGuard>().FirstOrDefaultAsync(predicate);
    }
    
    
}