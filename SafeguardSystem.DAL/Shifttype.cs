using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL;

public partial class Shifttype
{
    public Guid TypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public ulong IsDeleted { get; set; }

    public virtual ICollection<Securityshift> Securityshifts { get; set; } = new List<Securityshift>();
}
