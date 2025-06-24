using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.FriendRequest
{
    public class UpdateFriendRequestDto
    {
        public int Id { get; set; }
        public required RequestStatus Status { get; set; }
    }

}
