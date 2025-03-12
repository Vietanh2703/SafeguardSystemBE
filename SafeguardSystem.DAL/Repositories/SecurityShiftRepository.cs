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
    }
}
