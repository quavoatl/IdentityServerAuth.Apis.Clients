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
        public async Task<IActionResult> Index()
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

            tokenClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenResult.AccessToken);

            var userInfo = await tokenClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = tokenResult.AccessToken 
            });
            
            //var userInfo = await tokenClient.GetAsync("https://localhost:5005/connect/userinfo");
            //var userInfoContent = userInfo.Content.ReadAsStringAsync();

            
            

            return Ok();

            //retrieve access token
            // var authServerClient = _clientFactory.CreateClient();
            //
            // var discoveryDoc = await authServerClient.GetDiscoveryDocumentAsync("https://localhost:5005/");
            // var tokenResponse = await authServerClient.RequestClientCredentialsTokenAsync(
            //     new ClientCredentialsTokenRequest()
            //     {
            //         Address = discoveryDoc.TokenEndpoint,
            //         
            //         ClientId = "client_id",
            //         ClientSecret = "client_secret",
            //         
            //         Scope = "client_id_swagger_test",
            //     });
            //
            // //retrieve secret data
            // var apiClient = _clientFactory.CreateClient();
            // apiClient.SetBearerToken(tokenResponse.AccessToken);
            //
            // var responseMessage = await apiClient.GetAsync("https://localhost:5001/api/v1/limits");
            // var responseContent = await responseMessage.Content.ReadAsStringAsync();
            //
            // return Ok(new
            // {
            //     access_token = tokenResponse.AccessToken,
            //     message = responseContent
            // });
        }
    }
}