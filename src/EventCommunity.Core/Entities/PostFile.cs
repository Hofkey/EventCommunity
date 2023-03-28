using System.ComponentModel.DataAnnotations;

namespace EventCommunity.Core.Entities
{
    public class PostFile : BaseEntity
    {
        [Required(ErrorMessage = "File name is required!"), 
            MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author id is required!")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Post id is required!")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Upload date is required!")]
        public DateTime Uploaded { get; set; }

        [Required(ErrorMessage = "File extension is required!")]
        public string Extension { get; set; } = string.Empty;

        [Required(ErrorMessage = "File data must be provided!")]
        public byte[] File { get; set; } = Array.Empty<byte>();
    }
}
