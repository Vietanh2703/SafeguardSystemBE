using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class SecurityShiftRepository : GenericRepository<SecurityShift>, ISecurityShiftRepository
{
    private readonly SafeguardDbContext _context;

    public SecurityShiftRepository(SafeguardDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<SecurityShift> AddAsync(SecurityShift entity)
    {
        await _context.SecurityShifts.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<SecurityShift> GetByGuIdAsync(Guid id)
    {
        return await _context.SecurityShifts.FindAsync(id);
    }

    public async Task<SecurityShift> GetShiftByDetailAsync(Guid locationId, Guid teamId, Guid typeId)
    {
        return await _context.SecurityShifts.FindAsync(locationId, teamId, typeId);
    }
    
    public async Task<SecurityShift?> GetAsync(Expression<Func<SecurityShift, bool>> predicate)
    {
        return await _context.Set<SecurityShift>().FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<SecurityShift>> GetAllAsync(Expression<Func<SecurityShift, bool>> predicate = null)
    {
        if (predicate == null)
        {
            return await _context.Set<SecurityShift>().Include(shift => shift.Type).ToListAsync();
        }
        return await _context.Set<SecurityShift>().Where(predicate).Include(shift => shift.Type).ToListAsync();
    }

    public async Task<IEnumerable<SecurityShift>> GetShiftsByDateAsync(DateOnly date)
    {
        return await _context.Set<SecurityShift>()
            .Where(shift => shift.ShiftDate == date)
            .Include(shift => shift.Type) // Include related ShiftType
            .ToListAsync();
    }

}