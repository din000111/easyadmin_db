using Blazored.LocalStorage;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;


        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return _authenticationStateProvider.GetAuthenticationStateAsync();
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var serializedObject = JsonConvert.SerializeObject(loginModel);
            var response = await _httpClient.PostAsync("api/Accounts/login", new StringContent(serializedObject, Encoding.UTF8, "application/json"));
            var loginResult = new LoginResult { Successful = response.IsSuccessStatusCode};
            if (loginResult.Successful)
            {
                var content = await response.Content.ReadAsStringAsync();
                loginResult = JsonConvert.DeserializeObject<LoginResult>(content);
                if (loginResult.Token == null)
                {
                    loginResult.Successful = false;
                    loginResult.Error = "Сервер прислал пустой токен";
                }
                await _localStorage.SetItemAsync("authToken", loginResult.Token);
                ((EasyAdminAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);
            }
            else
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    loginResult = JsonConvert.DeserializeObject<LoginResult>(content);
                }
                catch (Exception)
                {
                    loginResult.Error = await response.Content.ReadAsStringAsync();
                }                
                Console.WriteLine(loginResult.Error);
            }
            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((EasyAdminAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
