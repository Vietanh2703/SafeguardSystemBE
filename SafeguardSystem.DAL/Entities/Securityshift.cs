using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class SecurityShift
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid ShiftId { get; set; }

    [ForeignKey("Location")]
    public Guid LocationId { get; set; }

    [ForeignKey("Team")]
    public Guid TeamId { get; set; }

    [ForeignKey("SecurityType")]
    public Guid TypeId { get; set; }

    public virtual Location Location { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual ShiftType Type { get; set; } = null!;
}
