using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using IdentityServerAuth.Data;
using IdentityServerAuth.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerAuth
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var resourceConfiguration = new Configuration();

            services.AddHttpClient();
            
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseSqlServer(
                    connectionString:
                    "Data Source=EN1410441\\SQLEXPRESS;Initial Catalog=CarInsUsers;Integrated Security=True");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(o => { o.Password.RequiredLength = 8; })
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
            });

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiScopes(resourceConfiguration.GetRegisteredScopes())
                .AddInMemoryIdentityResources(resourceConfiguration.GetRegisteredIdentityResources())
                .AddInMemoryApiResources(resourceConfiguration.GetRegisteredApis())
                .AddInMemoryClients(resourceConfiguration.GetRegisteredClients())
                .AddTestUsers(resourceConfiguration.GetRegisteredTestUsersResources().ToList())
                .AddProfileService<IdentityClaimProfileService>();
                
            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddTransient<IResourceOwnerPasswordValidator, OwnerPasswordValidator>();
            services.AddTransient<IProfileService, IdentityClaimProfileService>();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseIdentityServer();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}