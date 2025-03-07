using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Web.Controllers
{
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [Route("reports/all")]
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return StatusCode(reports.StatusCode, reports);
        }

        [Route("report/create")]
        [HttpPost]
        public async Task<IActionResult> CreateReport(string userId, [FromForm] CreateReportDTO reportDTO)
        {
            var response = await _reportService.CreateReportAsync(userId, reportDTO);
            return StatusCode(response.StatusCode, response);
        }
    }
}
