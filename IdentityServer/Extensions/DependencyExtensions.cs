using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Extensions
{
    public static class DependencyExtensions
    {
        public static IdentityBuilder AddMongoDbStoresAndServices(this IdentityBuilder builder)
        {
            AddStores(builder.Services, builder.UserType, builder.RoleType);
            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType, Type roleType)
        {
            services.AddMongoRepository();

            if (roleType != null)
            {
                services.TryAddScoped<IUserStore<User>, UserStore>();
                services.TryAddScoped<IRoleStore<Role>, RoleStore>();
                services.TryAddScoped<IClientStore, ClientStore>();
                services.TryAddScoped<IResourceStore, ResourcetStore>();
                services.TryAddScoped<IPersistedGrantStore, GrantStore>();
                services.TryAddScoped<IProfileService, ProfileService>();
            }
            else
            {
                services.TryAddScoped<IUserStore<User>, UserStore>();
            }
        }
    }
}
