using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

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


    public async Task<Guid> GetSecurityGuardRoleIdAsync()
    {
        return await _context.Roles
            .Where(r => r.RoleName == "Security Guard")
            .Select(r => r.RoleId)
            .FirstOrDefaultAsync();
    }

    public async Task<Role> GetRoleIdByNameAsync(string roleName)
    {
        return await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName);
    }
}