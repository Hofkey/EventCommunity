using System.ComponentModel.DataAnnotations;

namespace EventCommunity.Core.Entities
{
    public class Post : BaseEntity
    {
        [Required(ErrorMessage = "Title is required!"), MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The date when the post has been posted is required!")]
        public DateTime Posted { get; set; }

        [Required(ErrorMessage = "The author id is required!")]
        public int AuthorId { get; set; }

        public int RatingPositive { get; set; }

        public int RatingNegative { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual User Author { get; set; }

        public virtual ICollection<PostFile> Files { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
