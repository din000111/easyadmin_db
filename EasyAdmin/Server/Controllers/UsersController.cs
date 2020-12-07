using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAdmin.Server.Services.Authentication;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.Logging;

namespace EasyAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IAuthenticationService authService, IMemoryCache cache, ILogger<UsersController> logger)
        {
            _authService = authService;
            _cache = cache;
            _logger = logger;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _authService.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            List<User> users=new List<User>();
            if ((List<User>)_cache.Get(2) != null)
            {
                users = (List<User>)_cache.Get(2);
                _logger.LogInformation($"USERS CACHE YES : {users.Count}");
                return Ok(users);
            }
            else
            {
                _logger.LogInformation($"USERS CACHE NO : {users.Count}");
                users = _authService.GetUsersList();
                _cache.Set(2, users,new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                return Ok(users);
            }
        }
        [HttpGet]
        public async IAsyncEnumerable<User> GetUsersSafe()
        {
            List<User> users = _authService.GetUsersList();
            foreach (var user in users)
            {
                yield return user;
            }
        }
    }
}