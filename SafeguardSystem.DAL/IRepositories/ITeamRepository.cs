using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.IRepositories
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<PaginatedList<Team>> GetAllTeamsAsync(int pageNumber, int pageSize);
        Task<Team> GetTeamByNameAsync(string teamName);
        //Task<PaginatedList<SecurityGuard>> GetAllGuardsInTeamAsync(Guid teamId, int pageNumber, int pageSize);
        //Task<Team> GetTeamByGuardIdAsync(Guid guardId);
        //Task<bool> TeamHasGuardsAsync(Guid teamId);
        Task<bool> TeamExistsAsync(Guid teamId);
        Task<Team> CreateTeamAsync(Team team);
    }
}
