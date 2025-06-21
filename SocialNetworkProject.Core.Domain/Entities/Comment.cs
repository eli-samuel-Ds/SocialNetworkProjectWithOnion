using SocialNetworkProject.Core.Domain.Common;

namespace SocialNetworkProject.Core.Domain.Entities
{
    public class Comment : BasicEntity<int>
    {
        public required string Text { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required int AuthorId { get; set; }
        public required int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        public ApplicationUser? Author { get; set; }
        public Post? Post { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment>? Replies { get; set; }
    }
}
