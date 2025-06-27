namespace SocialNetworkProject.Core.Application.Dtos.Account
{
    public class UpdateProfileRequest
    {
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Password { get; set; }
    }
}
