using SocialNetworkProject.Core.Application.ViewModels.Battle;
using SocialNetworkProject.Core.Application.ViewModels.Home;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IBattleService
    {
        Task<BattleshipIndexViewModel> GetBattleshipIndexAsync(string userId);
        Task<StartNewGameViewModel> GetFriendsForNewGameAsync(string userId, string? searchTerm);
        Task<int> StartNewGameAsync(string player1Id, string player2Id);
        Task<ShipPlacementViewModel> GetShipPlacementViewModelAsync(int battleId, string userId);
        Task<bool> AreAllShipsPlaced(int battleId, string userId);
        Task<PlaceShipResultViewModel> PlaceShipAsync(PlaceShipViewModel vm);
        Task CheckAndStartGameAsync(int battleId);
        Task<GameViewModel> GetGameViewModelAsync(int battleId, string userId);
        Task<AttackResultViewModel> ProcessAttackAsync(int battleId, string attackerId, int x, int y);
        Task SurrenderGameAsync(int battleId, string surrenderingPlayerId);
        Task CheckForAbandonedGames();
        Task<GameHistoryDetailViewModel> GetGameHistoryDetailsAsync(int battleId, string userId);
        Task<UserBoardViewModel> GetUserBoardAsync(int battleId, string userId);
    }
}
