using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public partial class Business
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid BusinessId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime ContractExpiry { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual User User { get; set; } = null!;
}
