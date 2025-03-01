using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Web.Controllers
{
    public class GuardController : ControllerBase
    {
        private readonly IGuardService _securityGuardService;
        public GuardController(IGuardService securityGuardService)
        {
            _securityGuardService = securityGuardService;
        }

        [Route("guard/{guardId}")]
        [HttpGet]
        public async Task<IActionResult> GetGuardProfileById(Guid guardId)
        {
            var response = await _securityGuardService.GetGuardByIdAsync(guardId);
            return StatusCode(response.StatusCode, response);
        }

        [Route("guard/update/{guardId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateGuardProfile(Guid guardId,[FromBody] SecurityGuardDTO guardDTO)
        {
            var response = await _securityGuardService.UpdateGuardAsync(guardId, guardDTO);
            return StatusCode(response.StatusCode, response);
        }
    } 
}
