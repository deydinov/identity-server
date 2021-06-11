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
        public RepositoryConfiguration()
        {

        }
        public RepositoryConfiguration(IConfiguration configuration)
        {
            var conf = configuration.GetSection("Configuration").Get<RepositoryConfiguration>();
            Path = conf.Path;
        }
        public string Path { get; set; }
    }
}
