using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Contracts;
using IdentityServer4.Services;
using IdentityServerAuth.Contracts;
using IdentityServerAuth.Contracts.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAuth.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpClientFactory _clientFactory;
        
        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpClientFactory clientFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _clientFactory = clientFactory;
        }

        [HttpGet("/auth/login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() {ReturnUrl = returnUrl});
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Email.Equals(loginViewModel.Email));

            var loginSuccess = await _signInManager
                .PasswordSignInAsync(user, loginViewModel.Password, false, false);

            if (loginSuccess.Succeeded) return Redirect(loginViewModel.ReturnUrl);
            else if (loginSuccess.IsLockedOut)
            {
                //send mail with forget password
            }

            return View();
        }


        [HttpGet("/auth/register")]
        public IActionResult Register(string returnUrl)
        {
            var regModel = new RegisterViewModel() {ReturnUrl = returnUrl};

            return View(regModel);
        }

        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //check if the model is valid

            if (!ModelState.IsValid) return View(registerViewModel);

            var user = new IdentityUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username,
            };

            var registrationResponse = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (registrationResponse.Succeeded)
            {
                switch (registerViewModel.AccountType)
                {
                    case "Broker":
                        await _userManager.AddToRoleAsync(user, "BROKER");
                        break;
                    case "Customer":
                        await _userManager.AddToRoleAsync(user, "CUSTOMER");
                        break;
                    default: break;
                }

                await _signInManager.SignInAsync(user, false);

                return Redirect(registerViewModel.ReturnUrl);
            }

            return View();
        }
    }
}