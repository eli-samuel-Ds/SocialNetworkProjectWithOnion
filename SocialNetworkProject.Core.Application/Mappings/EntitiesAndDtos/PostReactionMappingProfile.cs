using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.PostReaction;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.EntitiesAndDtos
{
    public class PostReactionMappingProfile : Profile
    {
        public PostReactionMappingProfile()
        {
            CreateMap<PostReaction, PostReactionDto>().ReverseMap();

            CreateMap<PostReaction, CreatePostReactionDto>()
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Post, opt => opt.Ignore());
        }
    }
}
