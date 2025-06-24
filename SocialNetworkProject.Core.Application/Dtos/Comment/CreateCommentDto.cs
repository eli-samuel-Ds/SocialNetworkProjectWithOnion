namespace SocialNetworkProject.Core.Application.Dtos.Comment
{
    public class CreateCommentDto
    {
        public required string Text { get; set; }
        public required int AuthorId { get; set; }
        public required int PostId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
