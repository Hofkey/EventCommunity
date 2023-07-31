﻿using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models
{
    public class LoginData
    {
        [Required(ErrorMessage = "Email is required!"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = string.Empty;
    }
}
