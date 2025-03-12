using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class LocationDTO
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? image { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
