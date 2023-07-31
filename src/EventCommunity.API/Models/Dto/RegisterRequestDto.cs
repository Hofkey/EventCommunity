using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models.Dto
{
    public class NewRegisterRequestDto
    {
        [Required(ErrorMessage = "E-mail address is required!"),
            EmailAddress(ErrorMessage = "Invalid e-mail address!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Firstname is required!")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lastname is required!")]
        public string Lastname { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class RegisterRequestDto : NewRegisterRequestDto
    {
        public int Id { get; set; }
    }
}
