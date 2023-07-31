using System.ComponentModel.DataAnnotations;

namespace EventCommunity.API.Models.Dto
{
    public class CommunityEventDto
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public int CreatorId { get; set; }
    }
}
