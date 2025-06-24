namespace SocialNetworkProject.Core.Application.Dtos.Friendship
{
    public class CreateFriendshipDto
    {
        public required int UserId { get; set; }
        public required int FriendId { get; set; }
    }
}
