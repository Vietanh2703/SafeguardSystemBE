namespace SafeguardSystem.Common.DTOs;

public class ReportDTO
{
    public string ReportComment { get; set; }
    public string Sender { get; set; }
    public string ImageUrl { get; set; }
    public String Status { get; set; }
    public DateTime CreatedAt { get; set; }
}