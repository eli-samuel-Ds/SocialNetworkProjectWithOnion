using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.ViewModels.User;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterViewModel, RegisterRequest>()
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore()); 
        }
    }
}