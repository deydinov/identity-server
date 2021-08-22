using Abstractions;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Stores
{
    public class RoleStore : IRoleStore<Role>
    {
        private static IRepository _repo;
        private static ILookupNormalizer _keyNormalizer;

        public RoleStore(IRepository repository, ILookupNormalizer keyNormalizer)
        {
            _repo = repository;
            _keyNormalizer = keyNormalizer;

            if (_repo.CollectionEmpty<Role>())
            {
                _repo.AddMany(new List<Role>
                {
                    new Role("Administrator") { NormalizedName = "ADMINISTRATOR" },
                    new Role("User") { NormalizedName = "USER"}
                });
            }
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            await _repo.Add(role);
            
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            await _repo.Delete<Role>(x => x.Id == role.Id);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _repo.Single<Role>(x => x.Id == roleId);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _repo.Single<Role>(x => x.NormalizedName == normalizedRoleName);
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return await Task.FromResult(_keyNormalizer.NormalizeName(role.Name));
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return await Task.FromResult(role.Id);
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return await Task.FromResult(role.Name);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            role.NormalizedName = normalizedName;

            await Task.FromResult<object>(null);
        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            role.Name = roleName;

            await Task.FromResult<object>(null);
        }


        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            await _repo.Update(x => x.Id == role.Id, role);
            return IdentityResult.Success;
        }
    }
}
