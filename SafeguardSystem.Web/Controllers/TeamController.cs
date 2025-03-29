using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.BLL.Services;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers;

public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IGuardService _guardService;

    public TeamController(ITeamService teamService, IGuardService guardService)
    {
        _teamService = teamService;
        _guardService = guardService;
    }

    [Route("teams")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get all teams", Description = "Retrieve a paginated list of all teams.")]
    [SwaggerResponse(200, "Successfully retrieved list of teams.")]
    [SwaggerResponse(400, "Invalid request parameters.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetAllTeams(int pageNumber, int pageSize)
    {
        var teams = await _teamService.GetAllTeamsAsync(pageNumber, pageSize);
        return StatusCode(teams.StatusCode, teams);
    }

    [Route("team/{teamId}")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get team by ID", Description = "Retrieve details of a specific team by its ID.")]
    [SwaggerResponse(200, "Successfully retrieved team details.")]
    [SwaggerResponse(404, "Team not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetTeamById(Guid teamId)
    {
        var team = await _teamService.GetTeamByIdAsync(teamId);
        return StatusCode(team.StatusCode, team);
    }

    [Route("team")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new team", Description = "Create a new team with the provided details.")]
    [SwaggerResponse(201, "Successfully created new team.")]
    [SwaggerResponse(400, "Invalid team data.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> CreateTeam([FromBody] TeamDTO teamDTO)
    {
        var response = await _teamService.CreateTeamAsync(teamDTO);
        return StatusCode(response.StatusCode, response);
    }

    [Route("team/{teamId}")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update team details",
        Description = "Update the details of an existing team by its ID.")]
    [SwaggerResponse(200, "Successfully updated team details.")]
    [SwaggerResponse(400, "Invalid team data.")]
    [SwaggerResponse(404, "Team not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> UpdateTeam(Guid teamId, [FromBody] TeamDTO teamDTO)
    {
        var response = await _teamService.UpdateTeamAsync(teamId, teamDTO);
        return StatusCode(response.StatusCode, response);
    }

    [Route("team/delete/{teamId}")]
    [HttpPut]
    [SwaggerOperation(Summary = "Delete a team", Description = "Mark a team as deleted by its ID.")]
    [SwaggerResponse(200, "Successfully deleted team.")]
    [SwaggerResponse(404, "Team not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> DeleteTeam(Guid teamId)
    {
        var response = await _teamService.DeleteTeamAsync(teamId);
        return StatusCode(response.StatusCode, response);
    }

    [Route("guard")]
    [HttpPost]
    [SwaggerOperation(Summary = "Assign guard to team", Description = "Assign a guard to a specific team.")]
    [SwaggerResponse(200, "Successfully assigned guard to team.")]
    [SwaggerResponse(400, "Invalid guard or team ID.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> AssignGuardToTeam(Guid teamId, Guid guardId)
    {
        var response = await _teamService.AssignGuardToTeamAsync(teamId, guardId);
        return StatusCode(response.StatusCode, response);
    }

    [Route("guards")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get all guards", Description = "Retrieve a list of all guards.")]
    [SwaggerResponse(200, "Successfully retrieved list of guards.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetAllGuards()
    {
        var response = await _guardService.GetAllGuardAsync();
        return StatusCode(response.StatusCode, response);
    }

    [Route("guard/{teamGuardId}")]
    [HttpDelete]
    [SwaggerOperation(Summary = "Remove guard from team",
        Description = "Remove a guard from a specific team by the team-guard ID.")]
    [SwaggerResponse(200, "Successfully removed guard from team.")]
    [SwaggerResponse(404, "Guard not found in team.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> RemoveGuardFromTeam(Guid teamGuardId)
    {
        var response = await _teamService.RemoveGuardFromTeamAsync(teamGuardId);
        return StatusCode(response.StatusCode, response);
    }

    [Route("{teamId}/guards")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get guards in team",
        Description = "Retrieve a list of all guards in a specific team by the team ID.")]
    [SwaggerResponse(200, "Successfully retrieved list of guards in team.")]
    [SwaggerResponse(404, "Team not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetGuardsInTeam(Guid teamId)
    {
        var response = await _teamService.GetGuardsInTeamAsync(teamId);
        return StatusCode(response.StatusCode, response);
    }
}