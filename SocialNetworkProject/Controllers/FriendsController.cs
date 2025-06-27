using Microsoft.AspNetCore.Mvc;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Friend;
using SocialNetworkProject.Core.Application.ViewModels.Friendship;
using SocialNetworkProject.Helpers;
using SocialNetworkProject.Middlewares;

namespace SocialNetworkProject.Controllers
{
    [ServiceFilter(typeof(UserAuthorize))]
    public class FriendsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IFriendshipService _friendshipService;
        private readonly string _currentUserId;

        public FriendsController(IPostService postService, IFriendshipService friendshipService, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _friendshipService = friendshipService;
            _currentUserId = httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user")?.Id!;
        }

        public async Task<IActionResult> Index()
        {
            var friendsPosts = await _postService.GetPostsForFriendsFeedAsync(_currentUserId);
            var friendsList = await _friendshipService.GetAllFriendsAsync(_currentUserId);

            var viewModel = new FriendsIndexViewModel
            {
                FriendsPosts = friendsPosts,
                FriendsList = friendsList
            };

            return View(viewModel);
        }

        public async Task<IActionResult> UserPosts(string id)
        {
            var viewModel = await _postService.GetPostsByAuthorIdAsync(id);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var friends = await _friendshipService.GetAllFriendsAsync(_currentUserId);
            var friendToDelete = friends.FirstOrDefault(f => f.UserId == id);
            string friendName = friendToDelete?.UserName ?? "este amigo";

            var vm = new ConfirmationViewModel
            {
                Message = $"¿Está seguro que desea eliminar a {friendName}?",
                ConfirmationAction = "DeleteConfirmed",
                ConfirmationController = "Friends",
                ItemId = id 
            };
            return View("../Shared/_Confirmation", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _friendshipService.DeleteFriendshipAsync(_currentUserId, id);
            return RedirectToAction("Index");
        }
    }
}