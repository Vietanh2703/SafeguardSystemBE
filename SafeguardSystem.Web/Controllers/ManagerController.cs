using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers
{
    public class ManagerController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ISecurityshiftService _securityshiftService;
        public ManagerController(IReportService reportService, ISecurityshiftService securityshiftService)
        {
            _reportService = reportService;
            _securityshiftService = securityshiftService;
        }

        [Route("shift")]
        [HttpPost]
        [SwaggerOperation(Summary = "Assign shift", Description = "Assigns a shift to a team at a specific location.")]
        [SwaggerResponse(200, "Shift assigned successfully", typeof(ResponseDTO))]
        [SwaggerResponse(400, "Invalid request data")]
        [SwaggerResponse(404, "Location or team not found")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> AssignShift(Guid locationId, Guid teamId, Guid typeId)
        {
            var response = await _securityshiftService.AssignShiftAsync(locationId, teamId, typeId);
            return StatusCode(response.StatusCode, response);
        }


        [Route("shift/{shiftId}")]
        [HttpDelete]
        [SwaggerOperation(Summary = "Delete shift", Description = "Deletes a shift.")]
        [SwaggerResponse(200, "Shift deleted successfully", typeof(ResponseDTO))]
        [SwaggerResponse(400, "Invalid request data")]
        [SwaggerResponse(404, "Shift not found")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> DeleteShift([FromRoute] Guid shiftId)
        {
            var response = await _securityshiftService.DeleteShiftAsync(shiftId);
            return StatusCode(response.StatusCode, response);
        }


    }
}
