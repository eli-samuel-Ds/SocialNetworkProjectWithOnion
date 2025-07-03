using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class ShipViewModel
    {
        public int ShipId { get; set; }
        public ShipType Type { get; set; }
        public int Size { get; set; }
        public string Name => $"{Type} ({Size} celdas)";
    }
}
