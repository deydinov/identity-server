using Abstractions;
using IdentityServer.Models;
using IdentityServer.Models.Dto;
using IdentityServer.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly ClientStore _store;

        private readonly IIdentityServerInteractionService _interaction;


        public ClientController(ILogger<UserController> logger, UserManager<User> userManager, IIdentityServerInteractionService interaction, ClientStore store, IRepository repository)
        {
            _logger = logger;
            _interaction = interaction;
            _store = store;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewClient(ClientDto dto)
        {
            CancellationToken token = new();
            await _store.CreateAsync(null, token);
            return Ok();

        }
    }
}
