using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.Tokens
{
    public class ShiftToken
    {
        public Guid Id { get; set; }
        public Guid ShiftId { get; set; }
        public string Token { get; set; }
    }

}
