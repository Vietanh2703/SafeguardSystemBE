using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class ShiftType
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid TypeId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "TIME(6)")]
    public TimeSpan StartTime { get; set; }

    [Column(TypeName = "TIME(6)")]
    public TimeSpan EndTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<SecurityShift> SecurityShifts { get; set; } = new List<SecurityShift>();
}
