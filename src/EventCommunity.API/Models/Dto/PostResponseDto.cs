namespace EventCommunity.API.Models.Dto
{
    public class PostResponseDto
    {
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
        public int Count { get; set; }
    }
}
