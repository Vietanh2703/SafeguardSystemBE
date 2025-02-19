using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL;

public partial class Securityguard
{
    public Guid GuardId { get; set; }

    public string? IdentityNumber { get; set; }

    public string? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public Guid? UserId { get; set; }

    public Guid? TeamId { get; set; }

    public virtual ICollection<Shiftassignment> Shiftassignments { get; set; } = new List<Shiftassignment>();

    public virtual Team? Team { get; set; }

    public virtual User? User { get; set; }
}
