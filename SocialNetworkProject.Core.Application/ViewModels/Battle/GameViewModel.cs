namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class GameViewModel
    {
        public int BattleId { get; set; }
        public string CurrentUserId { get; set; }
        public string OpponentUserName { get; set; }
        public bool IsUserTurn { get; set; }
        public CellViewModel[,] AttackBoard { get; set; } = new CellViewModel[12, 12];
    }
}
