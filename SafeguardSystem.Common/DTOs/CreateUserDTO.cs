using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class CreateUserDTO
    {
        public string? Email { get; set; }
        public string? PassWord { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public Guid RoleId { get; set; }

    }
}
