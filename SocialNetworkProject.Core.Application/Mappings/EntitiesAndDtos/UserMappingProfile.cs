using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ReverseMap();

            CreateMap<ApplicationUser, CreateUserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Posts, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Reactions, opt => opt.Ignore())
                .ForMember(dest => dest.SentRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Friendships, opt => opt.Ignore())
                .ForMember(dest => dest.BattlesAsPlayer1, opt => opt.Ignore())
                .ForMember(dest => dest.BattlesAsPlayer2, opt => opt.Ignore())
                .ForMember(dest => dest.Ships, opt => opt.Ignore())
                .ForMember(dest => dest.Attacks, opt => opt.Ignore());

            CreateMap<ApplicationUser, UpdateUserDto>()
                .ReverseMap()
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Posts, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Reactions, opt => opt.Ignore())
                .ForMember(dest => dest.SentRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Friendships, opt => opt.Ignore())
                .ForMember(dest => dest.BattlesAsPlayer1, opt => opt.Ignore())
                .ForMember(dest => dest.BattlesAsPlayer2, opt => opt.Ignore())
                .ForMember(dest => dest.Ships, opt => opt.Ignore())
                .ForMember(dest => dest.Attacks, opt => opt.Ignore());
        }
    }
}
