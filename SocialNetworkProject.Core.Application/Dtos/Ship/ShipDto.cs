using SocialNetworkProject.Core.Application.Dtos.ShipPosition;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.Ship
{
    public class ShipDto
    {
        public int Id { get; set; }
        public int BattleId { get; set; }
        public int OwnerId { get; set; }
        public ShipType Type { get; set; }
        public int Size { get; set; }
        public bool IsPositioned { get; set; }
        public ICollection<ShipPositionDto> Positions { get; set; }
    }
}
