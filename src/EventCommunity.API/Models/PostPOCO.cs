using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models
{
    public class PostPOCO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required!"), MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The date when the post has been posted is required!")]
        public DateTime Posted { get; set;}

        [Required(ErrorMessage = "The author is required!")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UserPOCO Author { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int RatingPositive { get; set; }
        public int RatingNegative { get; set; }
        public List<FilePOCO> Files { get; set; } = new List<FilePOCO>();
    }
}
