using Microsoft.AspNetCore.Mvc;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Battle;
using SocialNetworkProject.Core.Application.ViewModels.Friendship;
using SocialNetworkProject.Helpers;
using SocialNetworkProject.Middlewares;

namespace SocialNetworkProject.Controllers
{
    [ServiceFilter(typeof(UserAuthorize))]
    public class BattleshipController : Controller
    {
        private readonly IBattleService _battleService;
        private readonly string _currentUserId;

        public BattleshipController(IBattleService battleService, IHttpContextAccessor httpContextAccessor)
        {
            _battleService = battleService;
            _currentUserId = httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user")?.Id!;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _battleService.GetBattleshipIndexAsync(_currentUserId);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> StartNewGame(string? searchTerm)
        {
            var vm = await _battleService.GetFriendsForNewGameAsync(_currentUserId, searchTerm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartNewGame(StartNewGameViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.SelectedOpponentId))
            {
                ModelState.AddModelError("SelectedOpponentId", "Debes seleccionar un amigo para desafiar.");
                var modelStateErrorVm = await _battleService.GetFriendsForNewGameAsync(_currentUserId, vm.SearchTerm);
                return View(modelStateErrorVm);
            }

            var battleId = await _battleService.StartNewGameAsync(_currentUserId, vm.SelectedOpponentId);
            return RedirectToAction("PlaceShips", new { id = battleId });
        }

        [HttpGet]
        public async Task<IActionResult> EnterGame(int id)
        {
            var battle = await _battleService.GetGameViewModelAsync(id, _currentUserId);
            if (battle == null) return RedirectToAction("Index");

            var areShipsPlaced = await _battleService.AreAllShipsPlaced(id, _currentUserId);
            if (!areShipsPlaced)
            {
                return RedirectToAction("PlaceShips", new { id = id });
            }
            return RedirectToAction("Game", new { id = id });
        }


        [HttpGet]
        public async Task<IActionResult> PlaceShips(int id)
        {
            var vm = await _battleService.GetShipPlacementViewModelAsync(id, _currentUserId);
            if (vm == null) return RedirectToAction("Index");

            if (vm.UnplacedShips.Count == 0 && !vm.IsWaitingForOpponent)
            {
                return RedirectToAction("Game", new { id = id });
            }

            if (TempData["ErrorMessage"] != null)
            {
                vm.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceShipAction(ShipPlacementViewModel vm)
        {
            var placeShipVm = new PlaceShipViewModel
            {
                BattleId = vm.BattleId,
                UserId = _currentUserId,
                SelectedShipId = vm.SelectedShipId,
                StartX = vm.StartX,
                StartY = vm.StartY,
                Direction = vm.Direction
            };

            var result = await _battleService.PlaceShipAsync(placeShipVm);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            return RedirectToAction("PlaceShips", new { id = vm.BattleId });
        }

        [HttpGet]
        public async Task<IActionResult> Game(int id)
        {
            var vm = await _battleService.GetGameViewModelAsync(id, _currentUserId);
            if (vm == null) return RedirectToAction("Index");

            var shipsPlaced = await _battleService.AreAllShipsPlaced(id, _currentUserId);
            if (!shipsPlaced)
            {
                return RedirectToAction("PlaceShips", new { id = id });
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attack([FromBody] AttackRequestViewModel request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var result = await _battleService.ProcessAttackAsync(request.BattleId, _currentUserId, request.X, request.Y);
            return Json(result);
        }

        [HttpGet]
        public IActionResult Surrender(int id)
        {
            var vm = new ConfirmationViewModel
            {
                ItemId = id.ToString(),
                Message = "¿Estás seguro de que deseas rendirte? La partida se dará por perdida.",
                ConfirmationAction = "SurrenderConfirmed",
                ConfirmationController = "Battleship"
            };
            return View("../Shared/_Confirmation", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SurrenderConfirmed(int id)
        {
            await _battleService.SurrenderGameAsync(id, _currentUserId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MyBoard(int id)
        {
            var vm = await _battleService.GetUserBoardAsync(id, _currentUserId);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GameResult(int id)
        {
            var vm = await _battleService.GetGameHistoryDetailsAsync(id, _currentUserId);
            return View(vm);
        }
    }
}
