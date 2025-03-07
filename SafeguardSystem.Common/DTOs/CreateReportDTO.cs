using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class CreateReportDTO
    {
        public string ReportComment { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
