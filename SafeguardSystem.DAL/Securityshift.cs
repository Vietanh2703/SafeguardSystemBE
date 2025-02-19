using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL;

public partial class Securityshift
{
    public Guid ShiftId { get; set; }

    public Guid? LocationId { get; set; }

    public Guid? TeamId { get; set; }

    public Guid? TypeId { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Shiftassignment> Shiftassignments { get; set; } = new List<Shiftassignment>();

    public virtual Team? Team { get; set; }

    public virtual Shifttype? Type { get; set; }
}
