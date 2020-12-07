using EasyAdmin.Shared.Common;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IAuthenticationService = EasyAdmin.Server.Services.Authentication.IAuthenticationService;

namespace EasyAdmin.Server.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IAuthenticationService _authService;


        public AccountsController(IAuthenticationService authService)
        {
            _authService = authService;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _authService.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [Route("login")]
        //[AllowAnonymous]
        public ActionResult<LoginResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user;
                try
                {
                    user = _authService.Login(loginModel.Username, loginModel.Password);
                }
                catch (Exception e)
                {
                    return BadRequest(new LoginResult { Successful = false, Token = null, Error = e.Message });
                }

                if (null != user)
                {
                    List<Claim> userClaims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Sam),
                        new Claim(JwtRegisteredClaimNames.GivenName, user.DisplayName),
                        new Claim(JwtRegisteredClaimNames.FamilyName, user.DistinguishedName),
                    };
                    if (user.IsAdmin)
                    {
                        //Дефолтный ClaimTypes.Role не работает во фронте с AuthorizedView. Поэтому эта хуета называется hueta. Извините
                        userClaims.Add(new Claim("hueta", "Admin"));
                        userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    foreach (var sub in user.Subordinates)
                    {
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, sub));
                    }
                    string token = _authService.CreateToken(userClaims);
                    return Ok(new LoginResult { Successful = true, Token = token });
                }
                else
                {
                    return BadRequest(new LoginResult { Successful = false, Error = "Неверно указан логин пользователя/пароль" });
                }
            }
            return BadRequest();
        }
    }
}