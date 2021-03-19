using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
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
        
    }
}