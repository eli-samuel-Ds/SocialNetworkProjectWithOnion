namespace SocialNetworkProject.Core.Application.Dtos.Account
{
    public class ResetPasswordRequest
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
    }
}
