using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class Location
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid LocationId { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    [ForeignKey("Business")] public Guid BusinessId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Business Business { get; set; }

    public virtual ICollection<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();

    public virtual ICollection<SecurityShift> SecurityShifts { get; set; } = new List<SecurityShift>();
}