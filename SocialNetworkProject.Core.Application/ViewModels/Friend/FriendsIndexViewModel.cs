namespace SocialNetworkProject.Core.Application.ViewModels.Friend
{
    public class FriendsIndexViewModel
    {
        public List<PostViewModel> FriendsPosts { get; set; } = new();
        public List<FriendViewModel> FriendsList { get; set; } = new();
    }
}
