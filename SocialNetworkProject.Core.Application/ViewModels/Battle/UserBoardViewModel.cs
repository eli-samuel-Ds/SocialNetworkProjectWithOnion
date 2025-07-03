namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class UserBoardViewModel
    {
        public int BattleId { get; set; }
        public CellViewModel[,] PlacementBoard { get; set; }
    }
}
