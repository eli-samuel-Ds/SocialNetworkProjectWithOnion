using SocialNetworkProject.Core.Domain.Common;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class PostReaction : BasicEntity<int>
    {
        public required ReactionType Reaction { get; set; }
        public required string UserId { get; set; }
        public required int PostId { get; set; }

        public ApplicationUser? User { get; set; }
        public Post? Post { get; set; }
    }
}
