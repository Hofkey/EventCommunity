using AutoMapper;
using EventCommunity.API.Models.Dto;
using EventCommunity.Core.Entities;

namespace EventCommunity.API.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PostFile, FileDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, NewUserDto>().ReverseMap();
            CreateMap<RegisterRequest, RegisterRequestDto>().ReverseMap();
            CreateMap<CommunityEvent, CommunityEventDto>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap()
                .ForMember(s => s.Files, t => t.Ignore());
        }
    }
}
