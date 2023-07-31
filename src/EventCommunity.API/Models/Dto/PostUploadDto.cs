using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models.Dto
{
    public class PostUploadDto
    {
        [Required(ErrorMessage = "Title is required!"), MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The date when the post has been posted is required!")]
        public DateTime Posted { get; set; }

        [Required(ErrorMessage = "The author is required!")]
        public int Author { get; set; }

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
