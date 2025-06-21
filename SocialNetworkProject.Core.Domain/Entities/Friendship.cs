using SocialNetworkProject.Core.Domain.Common;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class Friendship : BasicEntity<int>
    {
        public required int UserId { get; set; }
        public required int FriendId { get; set; }
        public required DateTime CreatedAt { get; set; }

        public ApplicationUser? User { get; set; }
        public ApplicationUser? FriendUser { get; set; }
    }
}
