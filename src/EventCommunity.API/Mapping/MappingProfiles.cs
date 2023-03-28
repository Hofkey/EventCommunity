using AutoMapper;
using EventCommunity.API.Models;
using EventCommunity.Core.Entities;

namespace EventCommunity.API.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<PostFile, FilePOCO>().ReverseMap();
            CreateMap<User, UserPOCO>().ReverseMap();
            CreateMap<Post, PostPOCO>().ReverseMap();
        }
    }
}
