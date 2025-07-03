namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class AttackResultViewModel
    {
        public bool IsHit { get; set; }
        public bool IsGameOver { get; set; }
        public string? WinnerId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
