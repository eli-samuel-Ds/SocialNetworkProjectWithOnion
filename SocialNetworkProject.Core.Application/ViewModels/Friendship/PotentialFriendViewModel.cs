namespace SocialNetworkProject.Core.Application.ViewModels.Friendship
{
    public class PotentialFriendViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int MutualFriendsCount { get; set; }
    }
}
