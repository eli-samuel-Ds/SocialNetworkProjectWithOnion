namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class BattleshipIndexViewModel
    {
        public List<ActiveGameViewModel> ActiveGames { get; set; } = new();
        public GameHistoryStatsViewModel GameHistoryStats { get; set; } = new();
        public List<GameHistorySummaryViewModel> GameHistory { get; set; } = new();
    }
}
