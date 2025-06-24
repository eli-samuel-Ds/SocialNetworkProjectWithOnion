using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.Friendship;
using SocialNetworkProject.Core.Domain.Entities;
 
namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class FriendshipMappingProfile : Profile
    {
        public FriendshipMappingProfile()
        {
            CreateMap<Friendship, FriendshipDto>().ReverseMap();

            CreateMap<Friendship, CreateFriendshipDto>()
                .ReverseMap()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.FriendUser, opt => opt.Ignore());
        }
    }
}
