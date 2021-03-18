using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServerAuth.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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


            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Auth/Login";
            });

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryApiScopes(resourceConfiguration.GetRegisteredScopes())
                .AddInMemoryIdentityResources(resourceConfiguration.GetRegisteredIdentityResources())
                .AddInMemoryApiResources(resourceConfiguration.GetRegisteredApis())
                .AddInMemoryClients(resourceConfiguration.GetRegisteredClients())
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}