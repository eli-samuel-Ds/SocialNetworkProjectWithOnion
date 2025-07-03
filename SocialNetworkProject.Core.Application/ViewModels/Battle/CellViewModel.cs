using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class CellViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CellStatus Status { get; set; } = CellStatus.Empty;
    }
}
