using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Friendship;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Helpers;
using SocialNetworkProject.Middlewares;

namespace SocialNetworkProject.Controllers
{
    [ServiceFilter(typeof(UserAuthorize))]
    public class FriendshipController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IMapper _mapper;
        private readonly string _currentUserId;

        public FriendshipController(IFriendshipService friendshipService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _friendshipService = friendshipService;
            _mapper = mapper;
            _currentUserId = httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user")?.Id!;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _friendshipService.GetFriendRequestsAsync(_currentUserId);
            return View(viewModel);
        }

        public async Task<IActionResult> Add(string? searchTerm)
        {
            var viewModel = await _friendshipService.GetPotentialFriendsAsync(_currentUserId, searchTerm);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFriendViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.SelectedUserId))
            {
                ModelState.AddModelError("SelectedUserId", "Debe seleccionar un usuario.");
                var modelStateErrorVm = await _friendshipService.GetPotentialFriendsAsync(_currentUserId, vm.SearchTerm);
                return View(modelStateErrorVm);
            }

            await _friendshipService.SendFriendRequestAsync(_currentUserId, vm.SelectedUserId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Accept(int id)
        {
            var requestInfo = await _friendshipService.GetFriendRequestInfoAsync(id, _currentUserId);
            var vm = new ConfirmationViewModel
            {
                ItemId = id,
                Message = $"¿Está seguro que desea aceptar la solicitud de amistad del usuario {requestInfo?.UserName}?",
                ConfirmationAction = "AcceptConfirmed",
                ConfirmationController = "Friendship"
            };
            return View("_Confirmation", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptConfirmed(int id)
        {
            await _friendshipService.RespondToFriendRequestAsync(id, _currentUserId, RequestStatus.Accepted);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var requestInfo = await _friendshipService.GetFriendRequestInfoAsync(id, _currentUserId);
            var vm = new ConfirmationViewModel
            {
                ItemId = id,
                Message = $"¿Está seguro que desea rechazar la solicitud de amistad del usuario {requestInfo?.UserName}?",
                ConfirmationAction = "RejectConfirmed",
                ConfirmationController = "Friendship"
            };
            return View("_Confirmation", vm);
        }

        [HttpPost]
        public async Task<IActionResult> RejectConfirmed(int id)
        {
            await _friendshipService.RespondToFriendRequestAsync(id, _currentUserId, RequestStatus.Rejected);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var requestInfo = await _friendshipService.GetFriendRequestInfoAsync(id, _currentUserId);
            var vm = new ConfirmationViewModel
            {
                ItemId = id,
                Message = $"¿Está seguro que desea eliminar la solicitud de amistad para {requestInfo?.UserName}?",
                ConfirmationAction = "DeleteConfirmed",
                ConfirmationController = "Friendship"
            };
            return View("_Confirmation", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _friendshipService.DeleteFriendRequestAsync(id, _currentUserId);
            return RedirectToAction("Index");
        }
    }
}