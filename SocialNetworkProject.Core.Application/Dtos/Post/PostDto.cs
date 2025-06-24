using Microsoft.AspNetCore.Mvc.Formatters;
using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;
using SocialNetworkProject.Core.Application.Dtos.Comment;
using SocialNetworkProject.Core.Application.Dtos.PostReaction;

namespace SocialNetworkProject.Core.Application.Dtos.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public MediaType MediaType { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public UserDto Author { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public ICollection<PostReactionDto> Reactions { get; set; }
    }
}
