using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.DAL.Entities
{
    public class TeamGuard
    {
        [Key] // Đánh dấu là khóa chính
        [Required(ErrorMessage = "Id is required")]
        public Guid TeamGuardId { get; set; }
        public Guid GuardId { get; set; }
        public SecurityGuard Guard { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }
}
