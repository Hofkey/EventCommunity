using EventCommunity.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models
{
    public class UserPOCO
    {
        [Required(ErrorMessage = "E-mail address is required!"),
            EmailAddress(ErrorMessage = "Invalid e-mail address!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Firstname is required!")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lastname is required!")]
        public string Lastname { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phonenumber!")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "User type is required!")]
        public UserRole UserRole { get; set; } = UserRole.Guest;
    }
}
