using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly SafeguardDbContext _context;

        public RoleRepository(SafeguardDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Role> GetByGuidAsync(Guid id)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == id);
        }
    }
}
