using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class ShipPlacementViewModel
    {
        public int BattleId { get; set; }
        public string UserId { get; set; }
        public List<ShipViewModel> UnplacedShips { get; set; } = new();
        public CellViewModel[,] Board { get; set; } = new CellViewModel[12, 12];
        public int? SelectedShipId { get; set; }
        public int? StartX { get; set; }
        public int? StartY { get; set; }
        public ShipDirection? Direction { get; set; }
        public bool IsWaitingForOpponent { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
