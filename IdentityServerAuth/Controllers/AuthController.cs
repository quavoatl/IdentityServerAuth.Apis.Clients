using System.Linq;
using System.Security.Claims;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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
            var loginSuccess = await _signInManager
                .PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (loginSuccess.Succeeded)
            {
                var user = _userManager.Users.SingleOrDefault(x => x.Email.Equals(loginViewModel.Username));

                // var userRoles = await _userManager.GetRolesAsync(user);
                //
                // foreach (var role in userRoles)
                // {
                //     _userManager.AddClaimAsync(user, new Claim("mareRol", role));
                // }
                
                await _signInManager.SignInAsync(user, false);

                return Redirect(loginViewModel.ReturnUrl);
            }

            else if (loginSuccess.IsLockedOut)
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
                await _userManager.AddClaimAsync(user, new Claim("mare.claim", "super.claim"));

                await _signInManager.SignInAsync(user, false);

                return Redirect(registerViewModel.ReturnUrl);
            }

            return View();
        }
    }
}