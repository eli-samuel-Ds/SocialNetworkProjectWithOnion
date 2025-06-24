using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.FriendRequest
{
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public UserDto Requester { get; set; }
        public UserDto Receiver { get; set; }
    }
}
