using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL.Entities;

public partial class Checkpoint
{
    public Guid CheckpointId { get; set; }

    public string Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public Guid? PlaceId { get; set; }

    public Guid LocationId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Location Location { get; set; }

    public virtual ICollection<Shiftassignment> Shiftassignments { get; set; } = new List<Shiftassignment>();
}
