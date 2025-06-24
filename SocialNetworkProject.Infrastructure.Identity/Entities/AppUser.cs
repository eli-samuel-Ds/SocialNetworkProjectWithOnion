using Microsoft.AspNetCore.Identity;

namespace SocialNetworkProject.Infrastructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
