using SocialNetworkProject.Core.Domain.Common;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class ShipPosition : BasicEntity<int>
    {
        public required int ShipId { get; set; }
        public required int X { get; set; }
        public required int Y { get; set; }

        public Ship? Ship { get; set; }
    }
}
