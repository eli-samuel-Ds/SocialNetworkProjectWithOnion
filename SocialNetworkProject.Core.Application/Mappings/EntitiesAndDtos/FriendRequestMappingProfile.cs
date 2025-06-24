using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.FriendRequest;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class FriendRequestMappingProfile : Profile
    {
        public FriendRequestMappingProfile()
        {
            CreateMap<FriendRequest, FriendRequestDto>().ReverseMap();

            CreateMap<FriendRequest, CreateFriendRequestDto>()
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RequestStatus.Pending))
                .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.RespondedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Requester, opt => opt.Ignore())
                .ForMember(dest => dest.Receiver, opt => opt.Ignore());

            CreateMap<FriendRequest, UpdateFriendRequestDto>()
                .ReverseMap()
                .ForMember(dest => dest.RequesterId, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RespondedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Requester, opt => opt.Ignore())
                .ForMember(dest => dest.Receiver, opt => opt.Ignore());
        }
    }
}
