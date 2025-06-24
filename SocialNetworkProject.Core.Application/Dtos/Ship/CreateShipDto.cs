using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.Ship
{
    public class CreateShipDto
    {
        public required int BattleId { get; set; }
        public required int OwnerId { get; set; }
        public required ShipType Type { get; set; }
    }
}
