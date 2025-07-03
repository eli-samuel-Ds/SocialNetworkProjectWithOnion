namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class GameHistorySummaryViewModel
    {
        public int BattleId { get; set; }
        public string OpponentUserName { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public double? DurationInHours => EndedAt.HasValue ? (EndedAt.Value - StartedAt).TotalHours : null;
        public string Result { get; set; } 
        public string WinnerUserName { get; set; } 
    }
}
