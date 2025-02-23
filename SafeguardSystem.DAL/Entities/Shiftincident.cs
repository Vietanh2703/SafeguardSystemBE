using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class ShiftIncident
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid IncidentId { get; set; }

    [ForeignKey("ShiftAssignment")]
    public Guid AssignmentId { get; set; }

    public string Description { get; set; }

    public string Envidence { get; set; }

    public DateTime IncidentTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ShiftAssignment Assignment { get; set; }
}
