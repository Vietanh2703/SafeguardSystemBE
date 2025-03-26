using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class CheckpointDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid LocationId { get; set; }
    }
}
