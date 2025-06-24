using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;

namespace SocialNetworkProject.Core.Application.Dtos.Friendship
{
    public class FriendshipDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto User { get; set; }
        public UserDto FriendUser { get; set; }
    }
}
