using System;
using System.Collections.Generic;

namespace SafeguardSystem.DAL.Entities;

public partial class Contract
{
    public Guid ContractId { get; set; }

    public string ContractCode { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal ContractValue { get; set; }

    public string Status { get; set; }

    public Guid TeamId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Team Team { get; set; }
}
