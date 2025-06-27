namespace SocialNetworkProject.Core.Application.ViewModels.Friendship
{
    public class FriendRequestViewModel
    {
        public List<FriendRequestInfoViewModel> ReceivedRequests { get; set; }
        public List<FriendRequestInfoViewModel> SentRequests { get; set; }

        public FriendRequestViewModel()
        {
            ReceivedRequests = new List<FriendRequestInfoViewModel>();
            SentRequests = new List<FriendRequestInfoViewModel>();
        }
    }
}
