using System.Linq.Expressions;
using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface ISecurityShiftRepository : IGenericRepository<SecurityShift>
{
    Task<SecurityShift> GetByGuIdAsync(Guid id);
    Task<SecurityShift> AddAsync(SecurityShift entity);
    Task<SecurityShift?> GetAsync(Expression<Func<SecurityShift, bool>> predicate);
    Task<IEnumerable<SecurityShift>> GetAllAsync(Expression<Func<SecurityShift, bool>> predicate = null);
    Task<IEnumerable<SecurityShift>> GetShiftsByDateAsync(DateOnly date);

}