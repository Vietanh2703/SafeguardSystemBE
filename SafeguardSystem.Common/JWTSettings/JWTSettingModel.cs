using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SafeguardSystem.Common.JWTSettings
{
    public class JWTSettingModel
    {
        private static IConfiguration _configuration;

        static JWTSettingModel()
        {
            // Initialize _configuration here, for example, using a configuration builder
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public JWTSettingModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// The Secret key of the jwt to generate access token.
        /// </summary>
        public static string SecretKey => _configuration["JWTSettings:SecretKey"];

        /// <summary>
        /// The expire days of the jwt to generate access token.
        /// </summary>
        public static int ExpireDayAcessToken => int.Parse(_configuration["JWTSettings:ExpireDayAcessToken"]);

        /// <summary>
        /// The expire days of the jwt to generate refresh token.
        /// </summary>
        public static int ExpireDayRefreshToken => int.Parse(_configuration["JWTSettings:ExpireDayRefreshToken"]);

        /// <summary>
        /// The issuer of the token.
        /// </summary>
        public static string Issuer => _configuration["JWTSettings:Issuer"];

        /// <summary>
        /// The audience of the token.
        /// </summary>
        public static string Audience => _configuration["JWTSettings:Audience"];
    }
}
