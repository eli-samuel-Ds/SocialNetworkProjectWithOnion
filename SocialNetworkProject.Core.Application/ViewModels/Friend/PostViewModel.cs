using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.ViewModels.Friend
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? MediaUrl { get; set; }
        public MediaType MediaType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public string AuthorUserName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
        public List<ReactionViewModel> Reactions { get; set; } = new();
    }
}
