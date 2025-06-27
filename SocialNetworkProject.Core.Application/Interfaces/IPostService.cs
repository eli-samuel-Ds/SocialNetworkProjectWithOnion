using SocialNetworkProject.Core.Application.ViewModels.Friend;
using SocialNetworkProject.Core.Application.ViewModels.Home;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetPostsForFriendsFeedAsync(string userId);
        Task<UserPostsViewModel> GetPostsByAuthorIdAsync(string authorId);
        Task<PostViewModel> AddPostAsync(SavePostViewModel vm, string authorId);
        Task UpdatePostAsync(SavePostViewModel vm, string authorId);
        Task DeletePostAsync(int postId, string authorId);
        Task AddCommentAsync(SaveCommentViewModel vm, string authorId);
        Task DeleteCommentAsync(int commentId, string authorId);
        Task AddOrUpdateReactionAsync(SaveReactionViewModel vm, string userId);
        Task<SavePostViewModel> GetPostByIdForEditAsync(int postId, string authorId);
    }
}
