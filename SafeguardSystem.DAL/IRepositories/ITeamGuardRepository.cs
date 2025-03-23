using System.Linq.Expressions;
using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface ITeamGuardRepository : IGenericRepository<TeamGuard>
{
    Task<TeamGuard> AddAsync(TeamGuard entity);
    Task<TeamGuard> RemoveAsync(TeamGuard entity);
    Task<bool> Any(Expression<Func<TeamGuard, bool>> predicate);
    Task<TeamGuard?> GetAsync(Expression<Func<TeamGuard, bool>> predicate);
    Task<IEnumerable<TeamGuard>> GetAllAsync(Expression<Func<TeamGuard, bool>> predicate);
}