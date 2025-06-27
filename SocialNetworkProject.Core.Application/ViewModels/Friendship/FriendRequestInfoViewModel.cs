namespace SocialNetworkProject.Core.Application.ViewModels.Friendship
{
    public class FriendRequestInfoViewModel
    {
        public int RequestId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime RequestDate { get; set; }
        public int MutualFriends { get; set; }
        public string? Status { get; set; }
    }
}
