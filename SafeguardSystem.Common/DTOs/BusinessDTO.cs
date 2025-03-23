using System.ComponentModel.DataAnnotations;

namespace SafeguardSystem.Common.DTOs;

public class BusinessDTO
{
    [Key] // Đánh dấu là khóa chính
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}