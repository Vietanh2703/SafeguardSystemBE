using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL.Entities;

public partial class Levelincident
{
    public Guid LevelId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Shiftincident> Shiftincidents { get; set; } = new List<Shiftincident>();
}
