namespace SocialNetworkProject.Core.Application.Dtos.ShipPosition
{
    public class CreateShipPositionDto
    {
        public required int ShipId { get; set; }
        public required int X { get; set; }
        public required int Y { get; set; }
    }
}
