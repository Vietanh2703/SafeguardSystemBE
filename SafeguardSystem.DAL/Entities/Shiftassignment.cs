using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL.Entities;

public partial class Shiftassignment
{
    public Guid AssignmentId { get; set; }

    public Guid ShiftId { get; set; }

    public Guid GuardId { get; set; }

    public Guid LocationId { get; set; }

    public Guid CheckpointId { get; set; }

    public virtual Checkpoint Checkpoint { get; set; }

    public virtual Securityguard Guard { get; set; }

    public virtual Location Location { get; set; }

    public virtual Securityshift Shift { get; set; }

    public virtual ICollection<Shiftincident> Shiftincidents { get; set; } = new List<Shiftincident>();
}
