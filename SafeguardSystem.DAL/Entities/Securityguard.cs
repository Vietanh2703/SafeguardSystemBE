using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class SecurityGuard
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid GuardId { get; set; }

    public string IdentityNumber { get; set; }

    public string Status { get; set; }

    public DateTime? StartDate { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }

    [ForeignKey("TeamId")]
    public Guid TeamId { get; set; }
    public virtual Team Team { get; set; }

    public virtual ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();

    public virtual User User { get; set; }
}
