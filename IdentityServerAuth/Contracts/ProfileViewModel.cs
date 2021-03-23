using System.Collections.Generic;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerAuth.Contracts
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }

        public int Expiry { get; set; }

        public ProfileViewModel()
        {

        }

        public ProfileViewModel(IdentityUser user, TokenResponse UToken = null)
        {
            Id = user.Id;
            Username = user.UserName;
            Email = user.Email;
            Token = UToken.AccessToken;
            Expiry = UToken.ExpiresIn;
        }

        public static IEnumerable<ProfileViewModel> GetUserProfiles(IEnumerable<IdentityUser> users)
        {
            var profiles = new List<ProfileViewModel> { };
            foreach (IdentityUser user in users)
            {
                profiles.Add(new ProfileViewModel(user));
            }

            return profiles;
        }
    }
}