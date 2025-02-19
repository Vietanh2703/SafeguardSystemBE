using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL.Entities;

public partial class Shifttyperoutine
{
    public int Id { get; set; }

    public int ShiftTypeId { get; set; }

    public int CheckpointId { get; set; }

    public TimeOnly TimeConstraintStart { get; set; }

    public TimeOnly TimeConstraintEnd { get; set; }

    public virtual Checkpoint Checkpoint { get; set; }

    public virtual Shifttype ShiftType { get; set; }
}
