using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL.Entities;

public partial class Business
{
    public Guid BusinessId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime ContractExpiry { get; set; }

    public bool IsDeleted { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual User User { get; set; } = null!;
}
