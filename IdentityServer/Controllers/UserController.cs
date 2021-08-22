using IdentityServer.Models;
using IdentityServer.Models.Dto;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityServerInteractionService _interaction;


        public UserController(ILogger<UserController> logger, UserManager<User> userManager, IIdentityServerInteractionService interaction)
        {
            _logger = logger;
            _userManager = userManager;
            _interaction = interaction;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] NewUserRequest model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
            };
            user.Claims = new List<Claim>
            {
                new Claim("name", model.UserName),
                new Claim("given_name", model.FirstName),
                new Claim("family_name", model.LastName),
                new Claim("email", model.Email)
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            _ = await _userManager.AddToRoleAsync(user, "Administrator");

            /* 
             role,
             sub,
             profile,
             locale,
             zoneinfo,
             birthdate,
             gender,
             website,
             picture,
             preferred_username,
             nickname,
             middle_name,
             given_name,
             family_name,
             name,
             updated_at,
             email_verified,
             email,
             address
         */

            return Ok();

        }
    }
}
