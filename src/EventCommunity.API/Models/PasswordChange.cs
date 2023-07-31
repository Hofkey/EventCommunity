using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models
{
    public class PasswordChange
    {
        [Required(ErrorMessage = "Old password needs to be provided!")]
        public string OldPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "New password needs to be provided!")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
