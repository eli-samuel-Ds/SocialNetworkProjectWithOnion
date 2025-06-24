using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;
using SocialNetworkProject.Core.Application.Dtos.Attack;
using SocialNetworkProject.Core.Application.Dtos.Ship;
using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.Dtos.Battle
{
    public class BattleDto
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public BattleStatus Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int? WinnerId { get; set; }
        public UserDto Player1 { get; set; }
        public UserDto Player2 { get; set; }
        public ICollection<ShipDto> Ships { get; set; }
        public ICollection<AttackDto> Attacks { get; set; }
    }
}
