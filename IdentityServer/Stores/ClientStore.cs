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
    public class ClientStore : IClientStore
    {
        private IRepository _repo;

        public ClientStore(IRepository repository)
        {
            _repo = repository;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var res = await _repo.Single<Client>(c => c.ClientId == clientId);
            return res;
        }
    }
}
