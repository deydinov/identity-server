using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    //Need to override IdentityUser to have possibility to store ue claims and roles inside the mongoDb document
    //In MongoDb we have only one document containing all user data
    public class User : IdentityUser
    {
        public IList<Claim> Claims { get; set; }
        public IList<Role> Roles { get; set; }
    }
}
