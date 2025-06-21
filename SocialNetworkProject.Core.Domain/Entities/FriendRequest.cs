using SocialNetworkProject.Core.Domain.Common;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class FriendRequest : BasicEntity<int>
    {
        public required int RequesterId { get; set; }
        public required int ReceiverId { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public required DateTime RequestedAt { get; set; }
        public DateTime? RespondedAt { get; set; }

        public ApplicationUser? Requester { get; set; }
        public ApplicationUser? Receiver { get; set; }
    }
}
