using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityResources
{
    public class Roles : IdentityResource
    {
        public const string identifier = "roles";

        public Roles()
        {
            Name = identifier;
            DisplayName = "Roles";
            Required = true;
            Enabled = true;

            UserClaims.Add("role");
        }
    }
}
