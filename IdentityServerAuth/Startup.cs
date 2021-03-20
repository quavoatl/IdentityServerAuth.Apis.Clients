using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Services;
using IdentityServerAuth.Data;
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
                .AddInMemoryApiScopes(resourceConfiguration.GetRegisteredScopes())
                .AddInMemoryIdentityResources(resourceConfiguration.GetRegisteredIdentityResources())
                .AddInMemoryApiResources(resourceConfiguration.GetRegisteredApis())
                .AddInMemoryClients(resourceConfiguration.GetRegisteredClients())
                .AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddTransient<IProfileService, IdentityClaimProfileService>();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}