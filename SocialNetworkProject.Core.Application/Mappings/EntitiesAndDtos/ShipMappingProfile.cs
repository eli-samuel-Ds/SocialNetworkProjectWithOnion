using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.Ship;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class ShipMappingProfile : Profile
    {
        public ShipMappingProfile()
        {
            CreateMap<Ship, ShipDto>().ReverseMap();

            CreateMap<Ship, CreateShipDto>()
                .ReverseMap()
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => (int)src.Type))
                .ForMember(dest => dest.IsPositioned, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Battle, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Positions, opt => opt.Ignore());
        }
    }
}
