using EasyAdmin.Server.Services.Mailer;
using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using EasyAdmin.Server.Services.Audit;
using EasyAdmin.Server.Services.Authentication;
using System.IO;
using Microsoft.EntityFrameworkCore;
using EasyAdmin.Server.Services.BackgroundTasking;

namespace EasyAdmin.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://localhost:46029/",
                        ValidAudience = "http://localhost:46030/",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOUR_EPIC_SECRET_KEY")),
                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = "hueta"
                    };
                });
            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>();
            
            services.AddMvc().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            //TODO изменено
            services.AddEntityFrameworkNpgsql().AddDbContext<EasyAdminContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ConnectionString"))
            );
            //services.AddEntityFrameworkSqlite().AddDbContext<EasyAdminContext>();


            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.Configure<Ldap>(Configuration.GetSection("ldap"));
            services.Configure<Secret>(Configuration.GetSection("Secret"));
            services.Configure<Mail>(Configuration.GetSection("Mail"));

            services.AddScoped<IMailerService, MailerService>();
            services.AddScoped<IAuthenticationService, LdapAuthenticationService>();
            services.AddHostedService<AuditServiceHostedService>();
            services.AddScoped<IAuditService, AuditService>();
            //services.AddScoped<IAuditService, AuditService>();

            //services.AddSingleton<IScheduledTask, AuditTask>();

            //services.AddScheduler2((sender, args) =>
            //{
            //    Console.Write(args.Exception.Message);
            //    args.SetObserved();
            //});
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddHttpClient();
            services.AddScoped<HttpClient>();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {



            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            app.UseStaticFiles();

            app.UseClientSideBlazorFiles<Client.Startup>();

            app.UseRouting();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });
            UpdateDatabase(app);
            //using (EasyAdminContext client = new EasyAdminContext())
            //{
            //    client.Database.EnsureCreated();
            //}

        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<EasyAdminContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

    }
}
