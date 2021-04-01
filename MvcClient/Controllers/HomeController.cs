using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Contracts.ResponseObjects;
using Newtonsoft.Json;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("/index")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Policy = "CustomerOnly")] //wont work, redirects to Account/AccessDenied, although the claim is there
        [Authorize(Roles = "Customer")] //wont work, redirects to Account/AccessDenied, although the claim is there
        public async Task<IActionResult> Secret()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var claims = User.Claims.ToList();
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            return View();
        }

        [HttpGet("/listlimits")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> ListLimits()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var getAllResponse = await client.GetAsync("https://localhost:5001/api/v1/limits");
            var getAllContent = await getAllResponse.Content.ReadFromJsonAsync<List<LimitResponse>>();

            return View("ListLimits", getAllContent);
        }
    }
}