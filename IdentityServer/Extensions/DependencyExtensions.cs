using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer.Stores;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDb.Extensions;
using MongoDB.Bson.Serialization;
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

            services.TryAddScoped<IUserStore<User>, UserStore>();
            services.TryAddScoped<IRoleStore<Role>, RoleStore>();
            services.TryAddScoped<IClientStore, ClientStore>();
            services.TryAddScoped<IResourceStore, ResourceStore>();
            services.TryAddScoped<IPersistedGrantStore, GrantStore>();
            services.TryAddScoped<IProfileService, ProfileService>();
            services.TryAddScoped<IConsentService , ProfileService>();

            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Role>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<IdentityResource>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Grant>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<ApiResource>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<ApiScope>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Client>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
