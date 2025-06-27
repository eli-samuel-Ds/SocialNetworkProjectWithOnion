using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task AddAsync(UserDto dto);
    }
}
