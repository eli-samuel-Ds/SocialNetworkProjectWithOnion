using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.ViewModels.User;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class ProfileMappingProfile : Profile
    {
        public ProfileMappingProfile()
        {
            CreateMap<EditProfileViewModel, UpdateProfileRequest>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.ProfilePictureFile, opt => opt.Ignore());

            CreateMap<ProfileDto, EditProfileViewModel>()
                 .ForMember(dest => dest.ProfilePictureFile, opt => opt.Ignore())
                 .ForMember(dest => dest.Password, opt => opt.Ignore())
                 .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore());
        }
    }
}
