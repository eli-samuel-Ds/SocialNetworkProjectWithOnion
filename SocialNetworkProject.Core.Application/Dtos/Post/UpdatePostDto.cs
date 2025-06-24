namespace SocialNetworkProject.Core.Application.Dtos.Post
{
    public class UpdatePostDto
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public string? MediaUrl { get; set; }
    }
}
