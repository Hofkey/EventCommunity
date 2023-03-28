using System.ComponentModel.DataAnnotations;

namespace EventCommunity.Core.Entities
{
    public class PostRating : BaseEntity
    {
        [Required(ErrorMessage = "Post id is required!")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "User id is required!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Positivity status is required!")]
        public bool Positive { get; set; }

        public virtual User? User { get; set; }

        public virtual Post? Post { get; set; }
    }
}
