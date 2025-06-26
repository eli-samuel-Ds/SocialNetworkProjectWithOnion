namespace SocialNetworkProject.Core.Application.Dtos.Account
{
    public class AuthenticationRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
