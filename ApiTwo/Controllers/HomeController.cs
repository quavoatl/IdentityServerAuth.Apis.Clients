using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("/index")]
        public async Task<string> Index()
        {
            var authServerClient = _clientFactory.CreateClient();

            var disco = await authServerClient.GetDiscoveryDocumentAsync("https://localhost:5005");

            if (disco.IsError) throw new Exception(disco.Error);

            var tokenClient = _clientFactory.CreateClient();

            var tokenResult = await tokenClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "broker_limits_rest_client_tests",
                ClientSecret = "secret",
                UserName = "user100@example.com",
                Password = "Password1234!",
                Scope = "openid roles"
                
            });

            return tokenResult.AccessToken;  //return token, use it in request header for the necessary resource


        }
    }
}