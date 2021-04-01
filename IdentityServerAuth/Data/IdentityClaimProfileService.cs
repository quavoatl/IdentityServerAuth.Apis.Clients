using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerAuth.Data
{
    public class IdentityClaimProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _claimsFactory;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityClaimProfileService(UserManager<IdentityUser> userManager,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            IdentityUser user = null;

            //to update 36 is the GUID for swagger login, else if for password grant integration tests
            if (sub.Length.Equals(36))user = await _userManager.FindByIdAsync(sub);
            else user = await _userManager.FindByNameAsync(sub);
            
            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            if (user.Email != null)
            {
                claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
                claims.Add(new Claim("userId", user.Id));
            }

            //Map roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (string r in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            //Map claims
            var identityClaims = await _userManager.GetClaimsAsync(user);
            if (identityClaims != null && identityClaims.Count > 0)
            {
                claims.AddRange(identityClaims);
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            bool userActive = false;
            var sub = context.Subject.GetSubjectId();
            IdentityUser user = null;

            //to update 36 is the GUID for swagger login, else if for password grant integration tests
            if (sub.Length.Equals(36))user = await _userManager.FindByIdAsync(sub);
            else user = await _userManager.FindByNameAsync(sub);
             
            if (user != null) userActive = true;
            context.IsActive = userActive;
        }
    }
}