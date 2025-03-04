using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.Enums
{
    public enum ApprovalStatus
    {
        [Description("Rejected")]
        Rejected = 1,
        [Description("Accepted")]
        Accepted = 0,
    }
}
