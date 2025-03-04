using SafeguardSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface ITeamGuardRepository : IGenericRepository<TeamGuard>
    {
        Task<TeamGuard> AddAsync(TeamGuard entity);
        Task<TeamGuard> RemoveAsync(TeamGuard entity);
        Task<bool> Any(Expression<Func<TeamGuard, bool>> predicate);
    }
}
