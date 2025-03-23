using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SafeguardSystem.Common.Enums;

public enum ApprovalStatus
{
    [Display(Name = "Rejected")] [Description("Rejected")]
    Rejected = 0,

    [Display(Name = "Accepted")] [Description("Accepted")]
    Accepted = 1
}