using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;
using SocialNetworkProject.Core.Application.ViewModels.User;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> UpdateProfileAsync(UpdateProfileRequest request);
        Task<EditProfileViewModel> GetProfileForEditAsync(string userId);
        Task<ProfileDto> GetProfileForEditDtoAsync(string userId);
        Task SetProfilePictureAsync(string userId, string profilePictureUrl);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest request, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<AuthenticationResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task SignOutAsync();
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string userId); 
    }
}