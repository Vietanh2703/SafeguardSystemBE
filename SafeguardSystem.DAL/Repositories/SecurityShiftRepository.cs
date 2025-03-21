using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Repositories
{
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
    }
}
