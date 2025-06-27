namespace SocialNetworkProject.Core.Application.Dtos.Account
{
    public class ProfileDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
