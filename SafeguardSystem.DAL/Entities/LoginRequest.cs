using System.ComponentModel.DataAnnotations;

namespace SafeguardSystem.DAL.Entities;

public class LoginRequest
{
    [Key] // Đánh dấu là khóa chính
    [Required(ErrorMessage = "Id is required")]
    public Guid RequestId { get; set; }

    public User User { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public DateTime DateSent { get; set; }
    public DateTime? DateApproved { get; set; }
    public string Status { get; set; }
    public string? Reason { get; set; }
}