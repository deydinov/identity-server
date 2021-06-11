using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public UserClaim()
        {

        }

        public UserClaim(Claim claim, IdentityUser user)
        {
            UserId = user.Id;
            InitializeFromClaim(claim);
        }
    }
}
