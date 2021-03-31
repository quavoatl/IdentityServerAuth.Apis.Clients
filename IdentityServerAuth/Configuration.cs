using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Contracts;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerAuth
{
    public class Configuration
    {
        private static readonly List<ClientClaim> _clientClaims = new List<ClientClaim>()
        {
            new ClientClaim(ClaimTypes.Role, "Broker")
        };

        private readonly IEnumerable<TestUser> _registeredUsers = new List<TestUser>()
        {
            new TestUser
            {
                SubjectId = "Broker",
                Username = "user100@example.com",
                Password = "Password1234!",
                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "Broker"),
                    new Claim("cacat", "mata")
                }
            },
        };

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
            new IdentityResource
            {
                Name = "roles.scope",
                UserClaims =
                {
                    "Broker",
                }
            },
            new IdentityResource(ClaimsHelpers.ROLES_KEY, "User role(s)", new List<string> {ClaimsHelpers.ROLE}),
        };

        private readonly IEnumerable<ApiResource> _registeredApis = new List<ApiResource>()
        {
            new ApiResource("client_id_swagger_test") {Scopes = {"client_id_swagger_test"}},
            new ApiResource("ApiOne") {Scopes = {"ApiOne"}},
            new ApiResource("ApiTwo") {Scopes = {"ApiTwo"}},
            new ApiResource("foo-api")
            {
                Scopes =
                {
                    "role"
                }
            },
            new ApiResource
            {
                Name = "api1",
                ApiSecrets = {new Secret("secret")},
                UserClaims =
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.GivenName,
                    JwtClaimTypes.FamilyName,
                    JwtClaimTypes.PreferredUserName
                },
                Description = "My API",
                DisplayName = "MyApi1",
                Enabled = true,
                Scopes = {"api1"}
            }
        };

        private readonly IEnumerable<ApiScope> _registeredScopes = new List<ApiScope>()
        {
            new ApiScope("client_id_swagger_test", "client_id_swagger_test"),
            new ApiScope("api1", "full access"),
            new ApiScope("ApiOne", "Api One Resource - mare secret"),
            new ApiScope("ApiTwo", "full access"),
            new ApiScope("roless", "full access")
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
                    "client_id_swagger_test",
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
                ClientSecrets = new List<Secret>() {new Secret("secretsecretsecret".ToSha256())},
                RequirePkce = true,
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
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
                ClientId = "broker_limits_rest_client",
                ClientSecrets = new List<Secret>() {new Secret("secret".ToSha256())},
                RequirePkce = true,
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
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
                ClientId = "broker_limits_rest_client_tests",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowAccessTokensViaBrowser = true,
                RequireClientSecret = true,
                AllowedCorsOrigins =
                {
                    "https://localhost:5001",
                    "https://localhost:44380"
                },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes =
                {
                    "ApiOne",
                    "ApiTwo",
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    ClaimsHelpers.ROLES_KEY
                },
                Claims = _clientClaims
            }
        };

        #region Getters

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

        public IEnumerable<TestUser> GetRegisteredTestUsersResources()
        {
            return _registeredUsers;
        }

        #endregion
    }
}