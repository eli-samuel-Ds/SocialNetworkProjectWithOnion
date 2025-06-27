using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Friend;
using SocialNetworkProject.Core.Domain.Entities;
using SocialNetworkProject.Core.Domain.Interfaces.Generic;

namespace SocialNetworkProject.Core.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _postRepo;
        private readonly IGenericRepository<Comment> _commentRepo;
        private readonly IGenericRepository<PostReaction> _reactionRepo;
        private readonly IFriendshipService _friendshipService;
        private readonly IAccountService _accountService;

        public PostService(
            IGenericRepository<Post> postRepo,
            IGenericRepository<Comment> commentRepo,
            IGenericRepository<PostReaction> reactionRepo,
            IFriendshipService friendshipService,
            IAccountService accountService)
        {
            _postRepo = postRepo;
            _commentRepo = commentRepo;
            _reactionRepo = reactionRepo;
            _friendshipService = friendshipService;
            _accountService = accountService;
        }

        public async Task<List<PostViewModel>> GetPostsForFriendsFeedAsync(string userId)
        {
            var friends = await _friendshipService.GetAllFriendsAsync(userId);
            var friendIds = friends.Select(f => f.UserId).ToHashSet();

            var allPosts = await _postRepo.GetAllAsync();
            var friendsPosts = allPosts.Where(p => friendIds.Contains(p.AuthorId));

            return await MapPostsToViewModels(friendsPosts, userId);
        }

        public async Task<UserPostsViewModel> GetPostsByAuthorIdAsync(string authorId)
        {
            var allPosts = await _postRepo.GetAllAsync();
            var authorPosts = allPosts.Where(p => p.AuthorId == authorId);

            var allUsers = await _accountService.GetAllUsersAsync();
            var author = allUsers.FirstOrDefault(u => u.Id == authorId);

            var viewModel = new UserPostsViewModel
            {
                FriendUserName = author?.UserName ?? "Usuario desconocido",
                Posts = await MapPostsToViewModels(authorPosts, authorId)
            };

            return viewModel;
        }

        private async Task<List<PostViewModel>> MapPostsToViewModels(IEnumerable<Post> posts, string currentUserId)
        {
            var allUsers = await _accountService.GetAllUsersAsync();

            var postViewModels = new List<PostViewModel>();
            foreach (var post in posts.OrderByDescending(p => p.CreatedAt))
            {
                var author = allUsers.FirstOrDefault(u => u.Id == post.AuthorId);
                postViewModels.Add(new PostViewModel
                {
                    Id = post.Id,
                    Content = post.Content,
                    MediaUrl = post.MediaUrl,
                    MediaType = post.MediaType,
                    CreatedAt = post.CreatedAt,
                    AuthorId = post.AuthorId,
                    AuthorUserName = author?.UserName ?? "Usuario desconocido",
                    AuthorProfilePictureUrl = author?.ProfilePictureUrl ?? "/images/default-profile.png",
                });
            }
            return postViewModels;
        }
    }
}
