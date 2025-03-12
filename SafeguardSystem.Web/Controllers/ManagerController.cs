using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;

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

        [Route("assign-shift")]
        [HttpPost]
        public async Task<IActionResult> AssignShift(Guid locationId, Guid teamId, Guid typeId)
        {
            var response = await _securityshiftService.AssignShiftAsync(locationId, teamId, typeId);
            return StatusCode(response.StatusCode, response);
        }



    }
}
