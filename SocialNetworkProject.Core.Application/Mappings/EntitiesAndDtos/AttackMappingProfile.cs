using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.Attack;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class AttackMappingProfile : Profile
    {
        public AttackMappingProfile()
        {
            CreateMap<Attack, AttackDto>().ReverseMap();

            CreateMap<Attack, CreateAttackDto>()
                .ReverseMap()
                .ForMember(dest => dest.IsHit, opt => opt.Ignore())
                .ForMember(dest => dest.AttackedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Battle, opt => opt.Ignore())
                .ForMember(dest => dest.Attacker, opt => opt.Ignore());
        }
    }
}
