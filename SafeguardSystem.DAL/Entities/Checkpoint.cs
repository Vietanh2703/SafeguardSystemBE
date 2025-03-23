using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class Checkpoint
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid CheckpointId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    [ForeignKey("Location")] public Guid LocationId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Location Location { get; set; }
}