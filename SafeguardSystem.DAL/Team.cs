using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL;

public partial class Team
{
    public Guid TeamId { get; set; }

    public string? Name { get; set; }

    public ulong? IsDeleted { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Securityguard> Securityguards { get; set; } = new List<Securityguard>();

    public virtual ICollection<Securityshift> Securityshifts { get; set; } = new List<Securityshift>();
}
