using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.ShipPosition;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class ShipPositionMappingProfile : Profile
    {
        public ShipPositionMappingProfile()
        {
            CreateMap<ShipPosition, ShipPositionDto>().ReverseMap();

            CreateMap<ShipPosition, CreateShipPositionDto>()
                .ReverseMap()
                .ForMember(dest => dest.Ship, opt => opt.Ignore());
        }
    }
}
