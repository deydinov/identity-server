using Abstractions;
using IdentityServer.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Stores
{
    public class ResourcetStore : IResourceStore
    {
        private IRepository _repo;

        public ResourcetStore(IRepository repository)
        {
            _repo = repository;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var res = await _repo.Where<ApiResource>(r => apiResourceNames.Contains(r.Name));
            return res;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var res = await _repo.Where<ApiResource>(r => r.Scopes.Any(s => scopeNames.Contains(s)));
            return res;
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var res = await _repo.Where<ApiScope>(a => scopeNames.Contains(a.Name));
            return res;
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var res = await _repo.Where<IdentityResource>(a => scopeNames.Contains(a.Name));
            return res;
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var result = new Resources(
                await _repo.All<IdentityResource>(),
                await _repo.All<ApiResource>(),
                await _repo.All<ApiScope>());

            return result;
        }
    }
}
