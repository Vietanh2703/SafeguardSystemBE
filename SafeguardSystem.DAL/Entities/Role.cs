using System.ComponentModel.DataAnnotations;

namespace SafeguardSystem.DAL.Entities;

public class Role
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}