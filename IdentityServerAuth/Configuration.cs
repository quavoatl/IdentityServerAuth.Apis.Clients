using System.Collections;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Contracts;
using IdentityServer4.Models;

namespace IdentityServerAuth
{
    public class Configuration
    {
        private readonly IEnumerable<IdentityResource> _registeredIdentityResources = new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "mare.scope",
                UserClaims =
                {
                    "mare.claim"
                }
            },
            new IdentityResource(ClaimsHelpers.ROLES_KEY, "User role(s)", new List<string> {ClaimsHelpers.ROLE})
        };

        private readonly IEnumerable<ApiResource> _registeredApis = new List<ApiResource>()
        {
            new ApiResource("ApiOne") {Scopes = {"ApiOne"}},
            new ApiResource("ApiTwo") {Scopes = {"ApiTwo"}},
            new ApiResource("api1") {Scopes = {"api1"}}
        };

        private readonly IEnumerable<ApiScope> _registeredScopes = new List<ApiScope>()
        {
            new ApiScope("api1", "full access"),
            new ApiScope("ApiOne", "full access"),
            new ApiScope("ApiTwo", "full access")
        };


        private IEnumerable<Client> _registeredClients = new List<Client>()
        {
            new Client
            {
                ClientId = "client_id",
                ClientSecrets = new List<Secret>() {new Secret("client_secret".ToSha256())},

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                    "ApiOne"
                }
            },
            new Client
            {
                ClientId = "client_id_mvc",
                ClientSecrets = new List<Secret>() {new Secret("client_secret_mvc".ToSha256())},

                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:5020/signin-oidc"},

                AllowedScopes =
                {
                    "ApiOne",
                    "ApiTwo",
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    "mare.scope",
                    ClaimsHelpers.ROLES_KEY
                },

                AlwaysIncludeUserClaimsInIdToken = true,

                RequireConsent = false
            },

            new Client
            {
                ClientId = "client_id_swagger_test",
                ClientSecrets = new List<Secret>() {new Secret("secret".ToSha256())},
                RequirePkce = true,
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris =
                {
                    "https://localhost:5030/swagger/oauth2-redirect.html"
                },
                AllowedCorsOrigins =
                {
                    "https://localhost:5030"
                },

                AllowedScopes =
                {
                    "ApiOne",
                    "ApiTwo",
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    "mare.scope",
                    ClaimsHelpers.ROLES_KEY
                },

                AlwaysIncludeUserClaimsInIdToken = true,
            },

            new Client
            {
                ClientId = "demo_api_swagger",
                ClientName = "Swagger UI for demo_api",
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowedGrantTypes = GrantTypes.Code,
                //RequirePkce = true,
                RequireClientSecret = false,

                RedirectUris =
                {
                    "https://localhost:5001/swagger/oauth2-redirect.html"
                },
                AllowedCorsOrigins =
                {
                    "https://localhost:5001"
                },
                AllowedScopes =
                {
                    "api1",
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    ClaimsHelpers.ROLES_KEY
                },
            },
        };

        public IEnumerable<ApiResource> GetRegisteredApis()
        {
            return _registeredApis;
        }

        public IEnumerable<Client> GetRegisteredClients()
        {
            return _registeredClients;
        }

        public IEnumerable<ApiScope> GetRegisteredScopes()
        {
            return _registeredScopes;
        }

        public IEnumerable<IdentityResource> GetRegisteredIdentityResources()
        {
            return _registeredIdentityResources;
        }
    }
}