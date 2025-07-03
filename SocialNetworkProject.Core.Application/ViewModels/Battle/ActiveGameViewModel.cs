namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class ActiveGameViewModel
    {
        public int BattleId { get; set; }
        public string OpponentUserName { get; set; }
        public DateTime StartedAt { get; set; }
        public double HoursSinceStart => (DateTime.UtcNow - StartedAt).TotalHours;
    }
}
