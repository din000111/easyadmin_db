using Blazored.Modal;
using EasyAdmin.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System;
using Blazorise;
using Blazored.Modal.Services;
using MatBlazor;

namespace EasyAdmin.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddScoped<AuthenticationStateProvider, EasyAdminAuthenticationStateProvider>();
            services.AddBlazoredModal();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IApiService, ApiService>();
            new System.IdentityModel.Tokens.Jwt.JwtPayload();
            services.AddAuthorizationCore();
            //для Blazorise - если используется что-то из Extensions, то добавляется EmptyProvider а не Bootstrap или что-то ещё
            services.AddEmptyProviders();
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
    public static class FileUtil
    {
        public async static Task SaveAs(IJSRuntime js, string filename, string data)
        {
            await js.InvokeAsync<object>(
                "saveAsFile",
                filename,
                data);
        }
    }
}
