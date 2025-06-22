using SocialNetworkProject.Core.Application.Dtos.Email;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto emailRequestDto);
    }
}
