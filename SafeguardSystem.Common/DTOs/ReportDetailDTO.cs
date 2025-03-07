using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class ReportDetailDTO
    {
        public string ReportComment { get; set; }
        public string ImageUrl { get; set; }
        public string? Status { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
