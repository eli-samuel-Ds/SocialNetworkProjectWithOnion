using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;

namespace SocialNetworkProject.Core.Application.Dtos.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public UserDto Author { get; set; }
        public ICollection<CommentDto> Replies { get; set; }
    }
}
