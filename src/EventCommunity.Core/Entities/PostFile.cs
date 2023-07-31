using System.ComponentModel.DataAnnotations;

namespace EventCommunity.Core.Entities
{
    public class PostFile : BaseEntity
    {
        [Required(ErrorMessage = "File name is required!"), 
            MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Post id is required!")]
        public int PostId { get; set; }

        [Required]
        public string Path { get; set; } = string.Empty;

        [Required(ErrorMessage = "Upload date is required!")]
        public DateTime Uploaded { get; set; }
    }
}
