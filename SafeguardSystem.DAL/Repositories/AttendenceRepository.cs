using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class AttendenceRepository : GenericRepository<Attendance>,IAttendenceRepository
{
    private readonly SafeguardDbContext _dbContext;
    
    public AttendenceRepository(SafeguardDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Attendance>> GetAllAsync(Expression<Func<Attendance, bool>> predicate)
    {
        return await _context.Set<Attendance>().Where(predicate).ToListAsync();
    }
    
    public async Task<Attendance> GetAttendanceByIdAsync(Guid id)
    {
        return await _context.Attendances.FirstOrDefaultAsync(a => a.AttendanceId == id);
    }

    public async Task<IEnumerable<Attendance>> GetShiftsByGuardIdAsync(Guid guardId)
    {
        return await _dbContext.Attendances.Where(a => a.GuardId == guardId).ToListAsync();
    }
    
}