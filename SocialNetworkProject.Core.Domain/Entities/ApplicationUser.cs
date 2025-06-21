using SocialNetworkProject.Core.Domain.Common;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class ApplicationUser : BasicEntity<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; } = false;
        
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostReaction>? Reactions { get; set; }
        public ICollection<FriendRequest>? SentRequests { get; set; }
        public ICollection<FriendRequest>? ReceivedRequests { get; set; }
        public ICollection<Friendship>? Friendships { get; set; }
        public ICollection<Battle>? BattlesAsPlayer1 { get; set; }
        public ICollection<Battle>? BattlesAsPlayer2 { get; set; }
        public ICollection<Ship>? Ships { get; set; }
        public ICollection<Attack>? Attacks { get; set; }
    }
}
