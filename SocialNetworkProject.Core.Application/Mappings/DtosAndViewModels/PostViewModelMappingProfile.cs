using AutoMapper;
using SocialNetworkProject.Core.Application.ViewModels.Home;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Core.Domain.Entities;

namespace SocialNetworkProject.Core.Application.Mappings.DtosAndViewModels
{
    public class PostViewModelMappingProfile : Profile
    {
        public PostViewModelMappingProfile()
        {
            CreateMap<SavePostViewModel, Post>()
                .ForMember(dest => dest.MediaUrl, opt => opt.Ignore()) 
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Reactions, opt => opt.Ignore());

            CreateMap<Post, SavePostViewModel>()
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore()) 
                .ForMember(dest => dest.ExistingMediaUrl, opt => opt.MapFrom(src => src.MediaUrl))
                .ForMember(dest => dest.VideoUrl, opt =>
                {
                    opt.MapFrom(src => src.MediaType == MediaType.YouTube
                                       ? $"https://www.youtube.com/watch?v={src.MediaUrl}"
                                       : string.Empty);
                });
        }
    }
}