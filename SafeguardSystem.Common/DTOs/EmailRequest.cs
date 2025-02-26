using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class EmailRequest
    {
        public string? Email { get; set; }       
        public string? Subject { get; set; }
        public string? EmailBody { get; set; }
    }
}
