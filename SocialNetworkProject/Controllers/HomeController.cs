using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Friendship;
using SocialNetworkProject.Core.Application.ViewModels.Home;
using SocialNetworkProject.Helpers;
using SocialNetworkProject.Middlewares;

namespace SocialNetworkProject.Controllers
{
    [ServiceFilter(typeof(UserAuthorize))]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly string _currentUserId;

        public HomeController(IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _currentUserId = httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user")?.Id!;
        }

        public async Task<IActionResult> Index()
        {
            var myPostsResult = await _postService.GetPostsByAuthorIdAsync(_currentUserId);
            var friendsPostsResult = await _postService.GetPostsForFriendsFeedAsync(_currentUserId);

            var viewModel = new HomeViewModel
            {
                MyPosts = myPostsResult.Posts,
                FriendsPosts = friendsPostsResult
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([Bind(Prefix = "SavePost")] SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var myPostsResult = await _postService.GetPostsByAuthorIdAsync(_currentUserId);
                var friendsPostsResult = await _postService.GetPostsForFriendsFeedAsync(_currentUserId);
                var viewModel = new HomeViewModel
                {
                    MyPosts = myPostsResult.Posts,
                    FriendsPosts = friendsPostsResult,
                    SavePost = vm
                };
                return View("Index", viewModel);
            }

            await _postService.AddPostAsync(vm, _currentUserId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _postService.GetPostByIdForEditAsync(id, _currentUserId);
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _postService.UpdatePostAsync(vm, _currentUserId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostByIdForEditAsync(id, _currentUserId);
            if (post == null)
            {
                return NotFound();
            }

            var vm = new ConfirmationViewModel
            {
                ItemId = id.ToString(),
                Message = "¿Está seguro que desea eliminar esta publicación?",
                ConfirmationAction = "DeleteConfirmed",
                ConfirmationController = "Home"
            };

            return View("../Shared/_Confirmation", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeletePostAsync(id, _currentUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(SaveCommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["CommentError"] = "El comentario no puede estar vacío.";
                return RedirectToAction("Index");
            }

            await _postService.AddCommentAsync(vm, _currentUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            await _postService.DeleteCommentAsync(commentId, _currentUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddReaction(SaveReactionViewModel vm)
        {
            await _postService.AddOrUpdateReactionAsync(vm, _currentUserId);
            return RedirectToAction("Index");
        }
    }
}