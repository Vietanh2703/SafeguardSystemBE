using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public DateTime DateSent { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }
}
