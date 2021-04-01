using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = "Cookies";
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = "https://localhost:5005";
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;
                    config.ResponseType = "code";

                    config.GetClaimsFromUserInfoEndpoint = true;

                    config.Scope.Add(ClaimsHelpers.ROLES_KEY);
                    config.Scope.Add("broker_limits_rest_api");
                    config.ClaimActions.MapUniqueJsonKey(ClaimsHelpers.ROLE,
                                                            ClaimsHelpers.ROLE,
                                                            ClaimsHelpers.ROLE);
                    config.TokenValidationParameters.RoleClaimType = ClaimsHelpers.ROLE;
                    //config.TokenValidationParameters.NameClaimType = "name";

                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}