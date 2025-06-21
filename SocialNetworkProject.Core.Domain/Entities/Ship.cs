using SocialNetworkProject.Core.Domain.Common;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class Ship : BasicEntity<int>
    {
        public required int BattleId { get; set; }
        public required int OwnerId { get; set; }
        public required ShipType Type { get; set; }
        public required int Size { get; set; }
        public bool IsPositioned { get; set; } = false;

        public Battle? Battle { get; set; }
        public ApplicationUser? Owner { get; set; }
        public ICollection<ShipPosition>? Positions { get; set; }
    }
}
