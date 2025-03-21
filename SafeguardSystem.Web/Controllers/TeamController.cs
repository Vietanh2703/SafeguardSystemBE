using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.IRepositories;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers
{
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [Route("teams")]
        [HttpGet]
        [SwaggerOperation]
        public async Task<IActionResult> GetAllTeams(int pageNumber, int pageSize)
        {
            var teams = await _teamService.GetAllTeamsAsync(pageNumber,pageSize);
            return StatusCode(teams.StatusCode,teams);
        }

        [Route("team/{teamId}")]
        [HttpGet]
        public async Task<IActionResult> GetTeamById(Guid teamId)
        {
            var team = await _teamService.GetTeamByIdAsync(teamId);
            return StatusCode(team.StatusCode, team);
        }

        [Route("team")]
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] TeamDTO teamDTO)
        {
            var response = await _teamService.CreateTeamAsync(teamDTO);
            return StatusCode(response.StatusCode, response);
        }

        [Route("team/{teamId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTeam(Guid teamId, [FromBody] TeamDTO teamDTO)
        {
            var response = await _teamService.UpdateTeamAsync(teamId, teamDTO);
            return StatusCode(response.StatusCode, response);
        }

        [Route("team/delete/{teamId}")]
        [HttpPut]
        public async Task<IActionResult> DeleteTeam(Guid teamId)
        {
            var response = await _teamService.DeleteTeamAsync(teamId);
            return StatusCode(response.StatusCode, response);
        }

        [Route("guard")]
        [HttpPost]
        public async Task<IActionResult> AssignGuardToTeam(Guid teamId, Guid guardId)
        {
            var response = await _teamService.AssignGuardToTeamAsync(teamId, guardId);
            return StatusCode(response.StatusCode, response);
        }

        [Route("guard/{teamGuardId}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveGuardFromTeam(Guid teamGuardId)
        {
            var response = await _teamService.RemoveGuardFromTeamAsync(teamGuardId);
            return StatusCode(response.StatusCode, response);
        }

        [Route(".{teamId}/guards")]
        [HttpGet]
        public async Task<IActionResult> GetGuardsInTeam(Guid teamId)
        {
            var response = await _teamService.GetGuardsInTeamAsync(teamId);
            return StatusCode(response.StatusCode, response);
        }


    }
}
