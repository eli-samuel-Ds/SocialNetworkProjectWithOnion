using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.Post
{
    public class CreatePostDto
    {
        public required string Content { get; set; }
        public MediaType MediaType { get; set; } = MediaType.None;
        public string? MediaUrl { get; set; }
        public required int AuthorId { get; set; }
    }
}
