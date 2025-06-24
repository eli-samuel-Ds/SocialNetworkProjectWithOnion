using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.PostReaction
{
    public class PostReactionDto
    {
        public int Id { get; set; }
        public ReactionType Reaction { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
