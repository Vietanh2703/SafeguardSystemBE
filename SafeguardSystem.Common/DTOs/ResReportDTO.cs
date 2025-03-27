using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class ResReportDTO
    {
        public string Respondent { get; set; }
        public string Reason { get; set; }
        public string? Status { get; set; }
    }
}
