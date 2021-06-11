using Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDb.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddMongoRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();

            return services;
        }

       
    }
}
