using System.ComponentModel.DataAnnotations;

namespace EventCommunity.Core.Entities
{
    public class RegisterRequest : BaseUser
    {
        [Required(ErrorMessage = "Token is required!")]
        public string Token { get; set; } = string.Empty;

        public bool Approved { get; set; }
    }
}
