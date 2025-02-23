using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class ShiftTypeRoutine
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid RoutineId { get; set; }

    [ForeignKey("ShiftType")]
    public Guid ShiftTypeId { get; set; }

    [ForeignKey("Checkpoint")]
    public Guid CheckpointId { get; set; }

    public TimeOnly TimeConstraintStart { get; set; }

    public TimeOnly TimeConstraintEnd { get; set; }

    public virtual Checkpoint Checkpoint { get; set; }

    public virtual ShiftType ShiftType { get; set; }
}
