using EventCommunity.API.Models.Dto;

namespace EventCommunity.API.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
