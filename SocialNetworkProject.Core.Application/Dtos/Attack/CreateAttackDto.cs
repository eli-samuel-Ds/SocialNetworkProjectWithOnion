namespace SocialNetworkProject.Core.Application.Dtos.Attack
{
    public class CreateAttackDto
    {
        public required int BattleId { get; set; }
        public required int AttackerId { get; set; }
        public required int X { get; set; }
        public required int Y { get; set; }
    }
}
