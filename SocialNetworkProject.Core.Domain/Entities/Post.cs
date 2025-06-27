using SocialNetworkProject.Core.Domain.Common;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class Post : BasicEntity<int>
    {
        public required string Content { get; set; }
        public MediaType MediaType { get; set; }
        public string? MediaUrl { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string AuthorId { get; set; } 

        public ApplicationUser? Author { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostReaction>? Reactions { get; set; }
    }
}
