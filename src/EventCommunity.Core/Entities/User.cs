using EventCommunity.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventCommunity.Core.Entities
{
    public class User : BaseUser
    {
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "User type is required!")]
        public UserRole UserRole { get; set; } = UserRole.Guest;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual ICollection<Post> Posts { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
