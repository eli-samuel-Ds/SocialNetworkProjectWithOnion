using SocialNetworkProject.Core.Domain.Common;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class Battle : BasicEntity<int>
    {
        public required string Player1Id { get; set; }
        public required string Player2Id { get; set; }
        public BattleStatus Status { get; set; } = BattleStatus.Setup;
        public required DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int? WinnerId { get; set; }

        public ApplicationUser? Player1 { get; set; }
        public ApplicationUser? Player2 { get; set; }
        public ICollection<Ship>? Ships { get; set; }
        public ICollection<Attack>? Attacks { get; set; }
    }
}
