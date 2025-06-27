using SocialNetworkProject.Core.Application.ViewModels.Friend;

namespace SocialNetworkProject.Core.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<PostViewModel> MyPosts { get; set; } = new();
        public List<PostViewModel> FriendsPosts { get; set; } = new();
        public SavePostViewModel SavePost { get; set; } = new();
    }
}
