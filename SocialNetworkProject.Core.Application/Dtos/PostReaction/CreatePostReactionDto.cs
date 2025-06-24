using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.PostReaction
{
    public class CreatePostReactionDto
    {
        public required ReactionType Reaction { get; set; }
        public required int UserId { get; set; }
        public required int PostId { get; set; }
    }
}
