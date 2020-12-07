using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace EasyAdmin.Client
{
    public class EasyAdminAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public EasyAdminAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                MarkUserAsLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            var token = parseToken(savedToken);
            if (token.ValidTo < DateTime.Now) MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            MarkUserAsAuthenticated(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwt", ClaimTypes.Name, "hueta")));
        }

        private JwtSecurityToken parseToken(string stringToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(stringToken) as JwtSecurityToken;
            var roles = tokenS.Claims.Where(x => x.Type == "role").Select(y => y.Value).ToArray();
            
            return tokenS;
        }

        public void MarkUserAsAuthenticated(JwtSecurityToken token)
        {
            foreach (var item in token.Claims)
            {
                if (item.Type == ClaimTypes.Role)
                {
                    Console.WriteLine(item.Value);
                }
            }
            //var roles = new ClaimsIdentity()
            // почему hueta - смотреть в EasyAdmin.Server.Controller.AccountsController
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwt", ClaimTypes.Name, "hueta"));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsAuthenticated(string stringToken)
        {
            var token = parseToken(stringToken);
            var identity = new ClaimsIdentity(token.Claims, "jwt", ClaimTypes.Name, "hueta");
            var authenticatedUser = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}