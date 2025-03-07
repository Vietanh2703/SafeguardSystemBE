using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class Business
{
    public Guid BusinessId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime ContractExpiry { get; set; }
    public bool IsDeleted { get; set; }
    public string UserId { get; set; }
    public virtual ICollection<Location> Locations { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<Contract> Contracts { get; set; } // Add this line to fix the error
}
