using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.Messages
{
    public class AdminApprovalMessage
    {
        public const string Complete = "Request has been approved";
        public const string Pending = "Request is processing";
        public const string Reject = "Request has been rejected";
        public const string Missing = "Cound not find this request";
        public const string Cancel = "Request has been canceled by user";
        public const string Same = "Request has been approved before";
    }
}
