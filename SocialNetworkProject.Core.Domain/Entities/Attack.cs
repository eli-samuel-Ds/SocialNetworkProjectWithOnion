using SocialNetworkProject.Core.Domain.Common;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class Attack : BasicEntity<int>
    {
        public required int BattleId { get; set; }
        public required string AttackerId { get; set; } 
        public required int X { get; set; }
        public required int Y { get; set; }
        public bool IsHit { get; set; }
        public required DateTime AttackedAt { get; set; }

        public Battle? Battle { get; set; }
        public ApplicationUser? Attacker { get; set; }
    }
}
