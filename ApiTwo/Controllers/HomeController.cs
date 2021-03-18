using System.Net.Http;
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
        public async Task<IActionResult> Index()
        {
            //retrieve access token
            var authServerClient = _clientFactory.CreateClient();

            var discoveryDoc = await authServerClient.GetDiscoveryDocumentAsync("https://localhost:5005/");
            var tokenResponse = await authServerClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = discoveryDoc.TokenEndpoint,
                    
                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    
                    Scope = "ApiOne",
                });
            
            //retrieve secret data
            var apiClient = _clientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var responseMessage = await apiClient.GetAsync("https://localhost:5010/secret");
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            
            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = responseContent
            });
        }
    }
}