using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class Contract
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid ContractId { get; set; }

    public string ContractCode { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal ContractValue { get; set; }

    public string Status { get; set; }

    [ForeignKey("Business")] public Guid BusinessId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Business Business { get; set; }
}