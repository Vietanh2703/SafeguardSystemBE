using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public partial class Checkpoint
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid CheckpointId { get; set; }

    public string Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    [ForeignKey("Location")]
    public Guid LocationId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Location Location { get; set; }

    public virtual ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
}
