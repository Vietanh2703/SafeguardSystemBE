using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllTeamsAsync(int pageNumber, int pageSize)
        {
            var paginatedTeams = await _unitOfWork.Teams.GetAllTeamsAsync(pageNumber, pageSize);
            if (paginatedTeams == null || !paginatedTeams.Any())
            {
                return new ResponseDTO("Empty team in list.", 200, false);
            }
            var teamDTOs = paginatedTeams.Where(t => !t.IsDeleted)
                                         .Select(t => new TeamDTO
                                         {
                                             Name = t.Name,
                                         }).ToList();
            return new ResponseDTO("Retrieve team: ", 200, true, new PaginatedList<TeamDTO>(teamDTOs, paginatedTeams.Count, pageSize, pageNumber));
        }

        //Search team by name
        public async Task<ResponseDTO> GetTeamByNameAsync(string teamName)
        {
            var results = await _unitOfWork.Teams.GetTeamByNameAsync(teamName);
            if (results == null)
            {
                return new ResponseDTO("Team not found", 404, false);
            }
            var teamDTO = new TeamDTO
            {
                Name = results.Name,
            };
            return new ResponseDTO("Team retrieved successfully", 200, true, teamDTO);
        }

        public async Task<ResponseDTO> GetAllGuardsInTeamAsync(Guid teamId, int pageNumber, int pageSize)
        {
            var paginatedGuards = await _unitOfWork.Teams.GetAllGuardsInTeamAsync(teamId, pageNumber, pageSize);
            if (paginatedGuards == null || !paginatedGuards.Any())
            {
                return new ResponseDTO("Empty guard in list.", 200, false);
            }
            var guardDTOs = paginatedGuards.Where(g => g.Status != "DELETED")
                                           .Select(g => new SecurityGuardDTO
                                           {
                                               FullName = g.User.FullName,
                                               PhoneNumber = g.User.Phone,
                                               IdentityNumber = g.IdentityNumber,
                                               Avatar = g.User.Avatar,
                                           }).ToList();
            return new ResponseDTO("Retrieve guard: ", 200, true, new PaginatedList<SecurityGuardDTO>(guardDTOs, paginatedGuards.Count, pageSize, pageNumber));
        }

        public async Task<ResponseDTO> GetTeamByIdAsync(Guid teamId)
        {
            var team = await _unitOfWork.Teams.GetByGuIdAsync(teamId);
            if (team == null)
            {
                return new ResponseDTO("Team not found", 404, false);
            }
            var teamDTO = new TeamDTO
            {
                Name = team.Name,
            };
            return new ResponseDTO("Team retrieved successfully", 200, true, teamDTO);
        }

        public async Task<ResponseDTO> CreateTeamAsync(TeamDTO teamDTO)
        {
            if (teamDTO == null)
            {
                return new ResponseDTO("Invalid team info", 400, false);
            }
            var team = new Team
            {
                Name = teamDTO.Name,
            };
            var result = await _unitOfWork.Teams.CreateTeamAsync(team);
            if (result == null)
            {
                return new ResponseDTO("Create team failed", 400, false);
            }
            return new ResponseDTO("Create team successfully", 200, true, teamDTO);
        }

        public async Task<ResponseDTO> UpdateTeamAsync(Guid teamId, TeamDTO teamDTO)
        {
            if (teamDTO == null)
            {
                return new ResponseDTO("Invalid team info", 400, false);
            }
            var result = await _unitOfWork.Teams.GetByGuIdAsync(teamId);
            if (result == null)
            {
                return new ResponseDTO("Team not found", 404, false);
            }
            result.Name = teamDTO.Name;
            await _unitOfWork.SaveChangeAsync();
            if (result == null)
            {
                return new ResponseDTO("Update team failed", 400, false);
            }
            return new ResponseDTO("Update team successfully", 200, true, teamDTO);
        }

        public async Task<ResponseDTO> DeleteTeamAsync(Guid teamId)
        {
            // Check if the team exists
            var teamExists = await _unitOfWork.Teams.TeamExistsAsync(teamId);
            if (!teamExists)
            {
                return new ResponseDTO("Team not found", 404, false);
            }

            // Check if the team has any guards
            var teamHasGuards = await _unitOfWork.Teams.TeamHasGuardsAsync(teamId);
            if (teamHasGuards)
            {
                return new ResponseDTO("Cannot delete team with existing guards", 400, false);
            }

            // Retrieve the team
            var team = await _unitOfWork.Teams.GetByGuIdAsync(teamId);
            if (team == null)
            {
                return new ResponseDTO("Team not found", 404, false);
            }

            // Set IsDeleted to true
            team.IsDeleted = true;
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Team deleted successfully", 200, true);
        }
    }
}
