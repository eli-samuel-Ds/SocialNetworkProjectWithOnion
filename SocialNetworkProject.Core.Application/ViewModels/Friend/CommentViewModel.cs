namespace SocialNetworkProject.Core.Application.ViewModels.Friend
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public string AuthorUserName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public List<CommentViewModel> Replies { get; set; } = new();
    }

}
