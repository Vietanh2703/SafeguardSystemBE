using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    private readonly SafeguardDbContext _context;

    public TeamRepository(SafeguardDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Team>> GetAllTeamsAsync(int pageNumber, int pageSize)
    {
        var query = _context.Teams.AsQueryable();
        return await PaginatedList<Team>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task<Team> GetTeamByNameAsync(string teamName)
    {
        return await _context.Teams
            .FirstOrDefaultAsync(team => team.Name == teamName);
    }

    //public async Task<PaginatedList<SecurityGuard>> GetAllGuardsInTeamAsync(Guid teamId, int pageNumber, int pageSize)
    //{
    //    var query = _context.SecurityGuards
    //        .Where(guard => guard.TeamId == teamId)
    //        .AsQueryable();
    //    return await PaginatedList<SecurityGuard>.CreateAsync(query, pageNumber, pageSize);
    //}

    //public async Task<Team> GetTeamByGuardIdAsync(Guid guardId)
    //{
    //    var guard = await _context.SecurityGuards
    //        .Include(guard => guard.Team)
    //        .FirstOrDefaultAsync(guard => guard.GuardId == guardId);
    //    return guard?.Team;
    //}

    //public async Task<bool> TeamHasGuardsAsync(Guid teamId)
    //{
    //    return await _context.SecurityGuards
    //        .AnyAsync(guard => guard.TeamId == teamId);
    //}

    public async Task<bool> TeamExistsAsync(Guid teamId)
    {
        return await _context.Teams.AnyAsync(t => t.TeamId == teamId && !t.IsDeleted);
    }

    public async Task<Team> CreateTeamAsync(Team team)
    {
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();
        return team;
    }
}