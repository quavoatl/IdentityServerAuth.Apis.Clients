using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerAuth.Extensions
{
    public class OwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public OwnerPasswordValidator(UserManager<IdentityUser> um)
        {
            UserManager = um;
        }

        public UserManager<IdentityUser> UserManager { get; }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await UserManager.FindByEmailAsync(context.UserName);

            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Username or password is incorrect");
                return;
            }

            var passwordValid = await UserManager.CheckPasswordAsync(user, context.Password);
            if (!passwordValid)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Username or password is incorrect");
                return;

            }
           
            var roles = await UserManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            context.Result = new GrantValidationResult(user.UserName, "password", claims);
        }
    }
}