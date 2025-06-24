namespace SocialNetworkProject.Core.Application.Dtos.Battle
{
    public class CreateBattleDto
    {
        public required int Player1Id { get; set; }
        public required int Player2Id { get; set; }
    }
}
