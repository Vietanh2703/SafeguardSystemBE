using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface ISecurityGuardRepository : IGenericRepository<SecurityGuard>
{
    Task<List<SecurityGuard>> GetAllGuardsAsync();
    Task<SecurityGuard> GetByIdAsync(Guid guardId);
}