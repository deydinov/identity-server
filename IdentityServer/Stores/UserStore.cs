using Abstractions;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Stores
{
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>, IUserClaimStore<User>, IUserRoleStore<User>, IUserEmailStore<User>
    {
        private IRepository _repo;
        private static ILookupNormalizer _keyNormalizer;
        private static IRoleStore<Role> _roleStore;

        public UserStore(IRepository repository, ILookupNormalizer keyNormalizer, IRoleStore<Role> roleStore, IConfiguration configuration)
        {
            _repo = repository;
            _keyNormalizer = keyNormalizer;
            _roleStore = roleStore;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            await _repo.Add(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            await _repo.Delete<User>(x => x.Id == user.Id);
            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _repo.Single<User>(x => x.Id == userId);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _repo.Single<User>(x => x.NormalizedUserName == normalizedUserName);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(_keyNormalizer.NormalizeName(user.UserName));
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(user.Id);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(user.UserName);
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.NormalizedUserName = normalizedName;

            await Task.FromResult<object>(null);
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.UserName = userName;

            await Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            await _repo.Update(x => x.Id == user.Id, user);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            //
        }

        // IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash;
            await Task.FromResult<object>(null);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));

        }

        // IUserClaimStore<User>

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var res = await _repo.Single<User>(x => x.Id == user.Id);

            return res.Claims;
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (claims.Any())
            {
                if (user.Claims == null)
                    user.Claims = new List<Claim>();

                foreach (var claim in claims.Where(x => !user.Claims.Contains(x)))
                {
                    user.Claims.Add(claim);
                }

                await _repo.Update(x => x.Id == user.Id, user);
            }
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var pos = user.Claims.IndexOf(claim);

            if (pos > -1)
            {
                user.Claims[pos] = newClaim;
                await _repo.Update(x => x.Id == user.Id, user);
            }
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            foreach (var claim in claims)
            {
                if (user.Claims.Contains(claim))
                {
                    user.Claims.Remove(claim);
                }
            }
            await _repo.Update(x => x.Id == user.Id, user);

        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var res = await _repo.Where<User>(x => x.Claims.Contains(claim));
            return res.ToList();
        }

        // IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (user.Roles == null)
                user.Roles = new List<Role>();

            var role = await _roleStore.FindByNameAsync(_keyNormalizer.NormalizeName(roleName), cancellationToken);

            if (role == null)
            {
                throw new ArgumentException(nameof(role));
            }

            if (!user.Roles.Contains(role))
            {
                user.Roles.Add(role);
                await _repo.Update(x => x.Id == user.Id, user);
            }
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = user.Roles?.FirstOrDefault(x => x.NormalizedName.Equals(roleName));

            if (role != null)
            {
                user.Roles.Remove(role);
                await _repo.Update(x => x.Id == user.Id, user);
            }
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(user.Roles?.Select(x => x.Name).ToList());
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.Roles == null)
            {
                return false;
            }

            return await Task.FromResult(user.Roles.Any(x => x.NormalizedName.Equals(roleName)));
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var res = (await _repo.Where<User>(x => x.Roles!=null && x.Roles.Any(r => r.NormalizedName == roleName))).ToList();

            return res;
        }

        //IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.Email = email;

            await Task.FromResult<object>(null);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.EmailConfirmed = confirmed;

            await Task.FromResult<object>(null);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _repo.Single<User>(x => x.NormalizedEmail == normalizedEmail);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await Task.FromResult(user.NormalizedEmail);
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.NormalizedEmail = normalizedEmail;

            await Task.FromResult<object>(null);
        }
    }
}
