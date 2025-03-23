namespace SafeguardSystem.Common.DTOs;

public class ReportDetailDTO
{
    public string ReportComment { get; set; }
    public string ImageUrl { get; set; }
    public string? Status { get; set; }
    public bool IsClosed { get; set; }
    public DateTime CreatedAt { get; set; }
}