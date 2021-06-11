﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class User : IdentityUser
    {
        public IList<Claim> Claims { get; set; }
        public IList<Role> Roles { get; set; }
    }
}
