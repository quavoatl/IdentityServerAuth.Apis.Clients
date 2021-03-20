using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    public class SecretController : Controller
    {
        [Authorize]
        [HttpGet("/secret")]
        public string Secret()
        {
            return "mare secret";
        }
    }
}