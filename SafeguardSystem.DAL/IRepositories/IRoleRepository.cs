using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role> GetByGuidAsync(Guid id);

    Task<Guid> GetSecurityGuardRoleIdAsync();
    Task<Role> GetRoleIdByNameAsync(string roleName);
}