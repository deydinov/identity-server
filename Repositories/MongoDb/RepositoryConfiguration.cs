using Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb
{
    public class RepositoryConfiguration : IRepositoryConfiguration
    {
        public RepositoryConfiguration(IConfiguration configuration)
        {
            var conf = configuration.GetSection("MongoDB");
            Path = conf.GetValue<string>("Path");
            UseSSL = conf.GetValue<bool>("UseSSL");
        }
        public string Path { get; private set; }
        public bool UseSSL { get; private set; }
    }
}
