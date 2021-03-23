using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAuth.Contracts.ViewModels
{
    public class RegisterViewModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public string AccountType { get; set; }
    }
}