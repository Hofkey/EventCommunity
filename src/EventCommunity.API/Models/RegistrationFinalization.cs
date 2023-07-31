using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models
{
    public class RegistrationFinalization
    {
        [Required(ErrorMessage = "Token is required!")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = string.Empty;
    }
}
