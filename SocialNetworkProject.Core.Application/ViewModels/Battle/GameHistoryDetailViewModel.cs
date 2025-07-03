namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class GameHistoryDetailViewModel
    {
        public int BattleId { get; set; }
        public CellViewModel[,] UserAttackBoard { get; set; }
        public CellViewModel[,] OpponentAttackBoard { get; set; }
        public CellViewModel[,] UserPlacementBoard { get; set; }
    }
}
