using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.Common.DTOs
{
    public class GoogleLoginDTO
    {
        /// <summary>
        /// The ID token received from Google after authentication.
        /// </summary>
        public required string? IdToken { get; set; }
    }
}
