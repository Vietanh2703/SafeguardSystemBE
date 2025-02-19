using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL.Entities;

public partial class Shiftincident
{
    public Guid IncidentId { get; set; }

    public Guid AssignmentId { get; set; }

    public string Description { get; set; }

    public string Envidence { get; set; }

    public Guid LevelId { get; set; }

    public DateTime IncidentTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Shiftassignment Assignment { get; set; }

    public virtual Levelincident Level { get; set; } = null!;
}
