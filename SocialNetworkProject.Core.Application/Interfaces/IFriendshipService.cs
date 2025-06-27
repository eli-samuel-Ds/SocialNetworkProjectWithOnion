using SocialNetworkProject.Core.Application.ViewModels.Friendship;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IFriendshipService
    {
        Task<FriendRequestViewModel> GetFriendRequestsAsync(string userId);
        Task<AddFriendViewModel> GetPotentialFriendsAsync(string currentUserId, string? searchTerm);
        Task SendFriendRequestAsync(string requesterId, string receiverId);
        Task RespondToFriendRequestAsync(int requestId, string currentUserId, RequestStatus newStatus);
        Task DeleteFriendRequestAsync(int requestId, string currentUserId);
        Task<FriendRequestInfoViewModel?> GetFriendRequestInfoAsync(int requestId, string currentUserId);
    }
}
