using SocialNetworkProject.Core.Application.Dtos.Account;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IAccountService
    {
        Task SetProfilePictureAsync(string userId, string profilePictureUrl);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest request, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<AuthenticationResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task SignOutAsync();
    }
}