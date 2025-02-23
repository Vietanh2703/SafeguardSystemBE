using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class Team
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid TeamId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    [ForeignKey("SecurityGuard")]
    public Guid GuardId { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<SecurityShift> SecurityShifts { get; set; } = new List<SecurityShift>();

    public virtual SecurityGuard Guard { get; set; }
}
