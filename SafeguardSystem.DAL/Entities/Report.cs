using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SafeguardSystem.DAL.Entities;

public class Report
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid ReportId { get; set; }

    public string Sender { get; set; }
    public string RoleName { get; set; }
    public string Respondent { get; set; }
    public string ReportComment { get; set; }
    public string ImageUrl { get; set; }
    public string? Status { get; set; }
    public string Reason { get; set; }
    public bool IsClosed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime AnsweredAt { get; set; }
    public string UserId { get; set; }

    [JsonIgnore] public virtual User User { get; set; }
}