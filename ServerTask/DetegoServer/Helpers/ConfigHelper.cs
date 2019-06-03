using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Helpers
{
    public class ConfigHelper
    {
        public static readonly ConfigHelper Instance = new ConfigHelper();
        private ConfigHelper()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            config.Bind(this);
            string path = AppContext.BaseDirectory + "/certificate.pfx";
            SecurityKey = File.ReadAllBytes(path);
        }
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public bool IncludeNullInJson { get; set; }
        public byte[] SecurityKey { get; internal set; }
    }
}
