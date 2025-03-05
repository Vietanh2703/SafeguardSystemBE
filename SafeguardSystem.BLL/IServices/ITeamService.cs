using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface ITeamService
    {
        Task<ResponseDTO> GetAllTeamsAsync(int pageNumber, int pageSize);
        Task<ResponseDTO> GetTeamByNameAsync(string teamName);
        Task<ResponseDTO> GetTeamByIdAsync(Guid teamId);
        Task<ResponseDTO> CreateTeamAsync(TeamDTO teamDTO);
        Task<ResponseDTO> UpdateTeamAsync(Guid teamId, TeamDTO teamDTO);
        Task<ResponseDTO> DeleteTeamAsync(Guid teamId);
        Task<ResponseDTO> AssignGuardToTeamAsync(Guid teamId, Guid guardId);
        Task<ResponseDTO> RemoveGuardFromTeamAsync(Guid teamGuardId);
        Task<ResponseDTO> GetGuardsInTeamAsync(Guid teamId);
    }
}
