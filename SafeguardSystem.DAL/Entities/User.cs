using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class User
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public string UserId { get; set; }  //Firebase UID

    [Required, MaxLength(100)]
    public string UserName { get; set; }

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }

    public string FullName { get; set; } 

    public string Avatar { get; set; } 

    public string Phone { get; set; }

    public DateTime? BirthDay { get; set; }

    public string? ActivationToken { get; set; }

    public DateTime? ActivationTokenExpiry { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public bool IsActive { get; set; }

    public bool IsLocked { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("Role")]
    public Guid RoleID { get; set; }

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();
    public virtual ICollection<SecurityGuard> SecurityGuards { get; set; } = new List<SecurityGuard>();
    public virtual Role Role { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
