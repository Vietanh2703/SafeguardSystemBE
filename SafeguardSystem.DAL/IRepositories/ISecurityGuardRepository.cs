using SafeguardSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface ISecurityGuardRepository : IGenericRepository<SecurityGuard>
    {
        Task<List<SecurityGuard>> GetAllGuardsAsync();
        Task<SecurityGuard> GetByIdAsync(Guid guardId);
    }
}
