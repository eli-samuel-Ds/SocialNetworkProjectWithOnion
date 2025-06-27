using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Infrastructure.Identity.Entities;

namespace SocialNetworkProject.Infrastructure.Identity.Mappings
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<AppUser, ProfileDto>().ReverseMap();
        }
    }
}
