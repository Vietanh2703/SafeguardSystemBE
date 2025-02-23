using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class ShiftAssignment
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid AssignmentId { get; set; }

    [ForeignKey("SecurityShift")]
    public Guid ShiftId { get; set; }

    [ForeignKey("SecurityGuard")]
    public Guid GuardId { get; set; }

    [ForeignKey("Location")]
    public Guid LocationId { get; set; }

    [ForeignKey("Checkpoint")]
    public Guid CheckpointId { get; set; }

    public virtual Checkpoint? Checkpoint { get; set; }

    public virtual SecurityGuard Guard { get; set; }

    public virtual Location? Location { get; set; }

    public virtual SecurityShift Shift { get; set; }

    public virtual ICollection<ShiftIncident> ShiftIncidents { get; set; } = new List<ShiftIncident>();
}
