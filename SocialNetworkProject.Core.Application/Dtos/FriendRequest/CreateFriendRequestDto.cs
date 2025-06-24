namespace SocialNetworkProject.Core.Application.Dtos.FriendRequest
{
    public class CreateFriendRequestDto
    {
        public required int RequesterId { get; set; }
        public required int ReceiverId { get; set; }
    }
}
