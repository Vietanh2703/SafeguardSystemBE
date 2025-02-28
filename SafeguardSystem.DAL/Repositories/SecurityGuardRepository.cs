using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Repositories
{
    class SecurityGuardRepository : GenericRepository<SecurityGuard>, ISecurityGuardRepository
    {
        private readonly SafeguardDbContext _context;

        public SecurityGuardRepository(SafeguardDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
