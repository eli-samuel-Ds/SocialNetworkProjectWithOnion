namespace SocialNetworkProject.Core.Application.ViewModels.Friend
{
    public class UserPostsViewModel
    {
        public string FriendUserName { get; set; }
        public List<PostViewModel> Posts { get; set; } = new();
    }
}
