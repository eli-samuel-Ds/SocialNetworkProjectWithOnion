using SocialNetworkProject.Core.Domain.Common;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class Friendship : BasicEntity<int>
    {
        public required string UserId { get; set; }
        public required string FriendId { get; set; }

        public required DateTime CreatedAt { get; set; }

        public ApplicationUser? User { get; set; }
        public ApplicationUser? FriendUser { get; set; }
    }
}
