namespace SocialNetworkProject.Core.Application.Dtos.Account
{
    public class ForgotPasswordRequest
    {
        public required string UserName { get; set; }
        public required string Origin { get; set; }
    }
}
