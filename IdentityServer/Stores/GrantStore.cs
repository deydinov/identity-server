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
    public class GrantStore : IPersistedGrantStore
    {
        private IRepository _repo;

        public GrantStore(IRepository repository)
        {
            _repo = repository;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            var res = await _repo.Where<PersistedGrant>(x => x.SubjectId == filter.SubjectId);
            if (filter.ClientId != null)
            {
                res = res.Where(x => x.ClientId == filter.ClientId);
            }
            if (filter.SessionId != null)
            {
                res = res.Where(x => x.SessionId == filter.SessionId);
            }
            if (filter.Type != null)
            {
                res = res.Where(x => x.Type == filter.Type);
            }

            return res;
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            return await _repo.Single<PersistedGrant>(x => x.Key == key);
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            _repo.Delete<PersistedGrant>(i => i.SubjectId == filter.SubjectId && i.ClientId == filter.ClientId && i.Type == filter.Type);
            return Task.FromResult(0);
        }

        public Task RemoveAsync(string key)
        {
            _repo.Delete<PersistedGrant>(i => i.Key == key);
            return Task.FromResult(0);
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            _repo.Add(grant);
            return Task.FromResult(0);
        }
    }
}
