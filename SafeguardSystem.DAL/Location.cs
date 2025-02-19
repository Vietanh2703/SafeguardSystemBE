using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL;

public partial class Location
{
    public Guid LocationId { get; set; }

    public string? Name { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public Guid? BusinessId { get; set; }

    public Guid? PlaceId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ulong? IsDeleted { get; set; }

    public virtual Business? Business { get; set; }

    public virtual ICollection<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();

    public virtual ICollection<Securityshift> Securityshifts { get; set; } = new List<Securityshift>();

    public virtual ICollection<Shiftassignment> Shiftassignments { get; set; } = new List<Shiftassignment>();
}
