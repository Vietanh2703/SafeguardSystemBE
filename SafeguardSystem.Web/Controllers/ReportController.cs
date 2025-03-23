using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers;

public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Route("reports")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get all reports", Description = "Retrieve a list of all reports.")]
    [SwaggerResponse(200, "Successfully retrieved list of reports.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetAllReports()
    {
        var reports = await _reportService.GetAllReportsAsync();
        return StatusCode(reports.StatusCode, reports);
    }

    [Route("report")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a report", Description = "Create a new report with the provided details.")]
    [SwaggerResponse(201, "Report created successfully.")]
    [SwaggerResponse(400, "Invalid report data.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> CreateReport(string userId, [FromForm] CreateReportDTO reportDTO)
    {
        var response = await _reportService.CreateReportAsync(userId, reportDTO);
        return StatusCode(response.StatusCode, response);
    }
}