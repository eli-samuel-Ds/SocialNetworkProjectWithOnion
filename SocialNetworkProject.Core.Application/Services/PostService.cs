using AutoMapper;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Friend;
using SocialNetworkProject.Core.Application.ViewModels.Home;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Core.Domain.Entities;
using SocialNetworkProject.Core.Domain.Interfaces.Generic;
using System.Text.RegularExpressions;

namespace SocialNetworkProject.Core.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _postRepo;
        private readonly IGenericRepository<Comment> _commentRepo;
        private readonly IGenericRepository<PostReaction> _reactionRepo;
        private readonly IFriendshipService _friendshipService;
        private readonly IAccountService _accountService;
        private readonly IFileUploader _fileUploader;
        private readonly IMapper _mapper;

        public PostService(
            IGenericRepository<Post> postRepo,
            IGenericRepository<Comment> commentRepo,
            IGenericRepository<PostReaction> reactionRepo,
            IFriendshipService friendshipService,
            IAccountService accountService,
            IFileUploader fileUploader,
            IMapper mapper)
        {
            _postRepo = postRepo;
            _commentRepo = commentRepo;
            _reactionRepo = reactionRepo;
            _friendshipService = friendshipService;
            _accountService = accountService;
            _fileUploader = fileUploader;
            _mapper = mapper;
        }

        public async Task<PostViewModel> AddPostAsync(SavePostViewModel vm, string authorId)
        {
            var post = _mapper.Map<Post>(vm);
            post.AuthorId = authorId;
            post.CreatedAt = DateTime.UtcNow;

            if (vm.MediaType == MediaType.Image && vm.ImageFile != null)
            {
                post.MediaUrl = _fileUploader.UploadFile(vm.ImageFile, authorId, "posts");
            }
            else if (vm.MediaType == MediaType.YouTube && !string.IsNullOrEmpty(vm.VideoUrl))
            {
                post.MediaUrl = GetYouTubeVideoId(vm.VideoUrl);
            }

            var createdPost = await _postRepo.AddAsync(post);
            return (await MapPostsToViewModels(new List<Post> { createdPost }, authorId)).First();
        }

        public async Task UpdatePostAsync(SavePostViewModel vm, string authorId)
        {
            var post = await _postRepo.GetByIdAsync(vm.Id);
            if (post == null || post.AuthorId != authorId)
            {
                throw new Exception("Publicación no encontrada o no autorizado para editar.");
            }

            post.Content = vm.Content;

            if (vm.MediaType == MediaType.Image && vm.ImageFile != null)
            {
                post.MediaUrl = _fileUploader.UploadFile(vm.ImageFile, authorId, "posts", true, vm.ExistingMediaUrl);
            }
            else if (vm.MediaType == MediaType.YouTube && !string.IsNullOrEmpty(vm.VideoUrl))
            {
                post.MediaUrl = GetYouTubeVideoId(vm.VideoUrl);
            }

            await _postRepo.UpdateAsync(post.Id, post);
        }

        public async Task<SavePostViewModel> GetPostByIdForEditAsync(int postId, string authorId)
        {
            var post = await _postRepo.GetByIdAsync(postId);
            if (post == null || post.AuthorId != authorId)
            {
                throw new Exception("Publicación no encontrada o no autorizado.");
            }
            var saveViewModel = _mapper.Map<SavePostViewModel>(post);
            saveViewModel.ExistingMediaUrl = post.MediaUrl;
            return saveViewModel;
        }

        public async Task DeletePostAsync(int postId, string authorId)
        {
            var post = await _postRepo.GetByIdAsync(postId);
            if (post != null && post.AuthorId == authorId)
            {
                await _postRepo.DeleteAsync(postId);
            }
        }

        public async Task AddCommentAsync(SaveCommentViewModel vm, string authorId)
        {
            var comment = new Comment
            {
                Id = default,
                Text = vm.Text,
                AuthorId = authorId,
                PostId = vm.PostId,
                ParentCommentId = vm.ParentCommentId,
                CreatedAt = DateTime.UtcNow
            };
            await _commentRepo.AddAsync(comment);
        }

        public async Task DeleteCommentAsync(int commentId, string authorId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId);
            if (comment != null && comment.AuthorId == authorId)
            {
                await _commentRepo.DeleteAsync(commentId);
            }
        }

        public async Task AddOrUpdateReactionAsync(SaveReactionViewModel vm, string userId)
        {
            var existingReactions = await _reactionRepo.GetAllAsync();
            var existingReaction = existingReactions.FirstOrDefault(r => r.PostId == vm.PostId && r.UserId == userId);

            if (existingReaction != null)
            {
                if (existingReaction.Reaction == vm.ReactionType)
                {
                    await _reactionRepo.DeleteAsync(existingReaction.Id);
                }
                else
                {
                    existingReaction.Reaction = vm.ReactionType;
                    await _reactionRepo.UpdateAsync(existingReaction.Id, existingReaction);
                }
            }
            else
            {
                await _reactionRepo.AddAsync(new PostReaction { Id = default, PostId = vm.PostId, UserId = userId, Reaction = vm.ReactionType });
            }
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

        private string GetYouTubeVideoId(string url)
        {
            var regex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?&]*))");
            var match = regex.Match(url);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
