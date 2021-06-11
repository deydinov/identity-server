using IdentityServer.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var user = new User { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
            //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", "user"));

           // await _userManager.AddToRoleAsync(user, "users");

            return Ok();

        }
    }
}
