using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class AssignShiftDTO
    {
        public Guid LocationId { get; set; }
        public Guid TeamId { get; set; }
        public Guid TypeId { get; set; }
        public DateOnly ShiftDate { get; set; }
    }
}
