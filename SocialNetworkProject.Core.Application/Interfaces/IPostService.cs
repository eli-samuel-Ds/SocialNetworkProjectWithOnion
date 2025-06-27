using SocialNetworkProject.Core.Application.ViewModels.Friend;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetPostsForFriendsFeedAsync(string userId);
        Task<UserPostsViewModel> GetPostsByAuthorIdAsync(string authorId);
    }
}
