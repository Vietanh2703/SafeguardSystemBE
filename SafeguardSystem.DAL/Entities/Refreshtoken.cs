using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class RefreshToken
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid RefreshTokenId { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public string? RefreshTokenKey { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual User User { get; set; }
}
