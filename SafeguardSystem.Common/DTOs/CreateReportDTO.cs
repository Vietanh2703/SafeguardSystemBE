using Microsoft.AspNetCore.Http;

namespace SafeguardSystem.Common.DTOs;

public class CreateReportDTO
{
    public string ReportComment { get; set; }
    public IFormFile ImageFile { get; set; }
}