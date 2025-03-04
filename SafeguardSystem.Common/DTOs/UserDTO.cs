using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class UserDTO
    {
        public string? IdentityNumber { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
