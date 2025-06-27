namespace SocialNetworkProject.Core.Application.Dtos.FriendRequest
{
    public class CreateFriendRequestDto
    {
        public required string RequesterId { get; set; }
        public required string ReceiverId { get; set; }
    }
}
