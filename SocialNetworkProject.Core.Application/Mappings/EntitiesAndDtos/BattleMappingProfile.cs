using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.Battle;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class BattleMappingProfile : Profile
    {
        public BattleMappingProfile()
        {
            CreateMap<Battle, BattleDto>().ReverseMap();

            CreateMap<Battle, CreateBattleDto>()
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BattleStatus.Setup))
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.EndedAt, opt => opt.Ignore())
                .ForMember(dest => dest.WinnerId, opt => opt.Ignore())
                .ForMember(dest => dest.Player1, opt => opt.Ignore())
                .ForMember(dest => dest.Player2, opt => opt.Ignore())
                .ForMember(dest => dest.Ships, opt => opt.Ignore())
                .ForMember(dest => dest.Attacks, opt => opt.Ignore());
        }
    }
}
