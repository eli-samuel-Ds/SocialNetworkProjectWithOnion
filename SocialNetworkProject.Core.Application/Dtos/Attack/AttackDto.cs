namespace SocialNetworkProject.Core.Application.Dtos.Attack
{
    public class AttackDto
    {
        public int Id { get; set; }
        public int BattleId { get; set; }
        public int AttackerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsHit { get; set; }
        public DateTime AttackedAt { get; set; }
    }
}
