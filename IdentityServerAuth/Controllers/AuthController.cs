using System.Linq;
using System.Threading.Tasks;
using IdentityServerAuth.Contracts;
using IdentityServerAuth.Contracts.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAuth.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("/auth/login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() {ReturnUrl = returnUrl});
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            //check if the model is valid

            var cacat = _userManager.Users.Where(x => x.Email.Contains(".com")).ToList().FirstOrDefault();
            

            var result =
                await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false,
                    false);

            if (result.Succeeded)
            {
                return Redirect(loginViewModel.ReturnUrl);
            }
            else if (result.IsLockedOut)
            {
                //send mail with forget password
            }

            return View();
        }


        [HttpGet("/auth/register")]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel() {ReturnUrl = returnUrl});
        }

        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //check if the model is valid

            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = new IdentityUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username,
            };
            var registrationResponse = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (registrationResponse.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return Redirect(registerViewModel.ReturnUrl);
            }

            return View();
        }
    }
}