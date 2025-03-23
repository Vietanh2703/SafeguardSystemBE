using System.Linq.Expressions;
using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface IAttendenceRepository : IGenericRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetShiftsByGuardIdAsync(Guid guardId);
    Task<IEnumerable<Attendance>> GetAllAsync(Expression<Func<Attendance, bool>> predicate);
}