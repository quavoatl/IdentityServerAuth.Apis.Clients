using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    public class SecretController : Controller
    {
        [HttpGet("/secret")]
        [Authorize]
        public string Secret()
        {
            return "mare secret";
        }
    }
}