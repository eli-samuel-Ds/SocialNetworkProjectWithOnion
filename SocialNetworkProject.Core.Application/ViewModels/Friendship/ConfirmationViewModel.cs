namespace SocialNetworkProject.Core.Application.ViewModels.Friendship
{
    public class ConfirmationViewModel
    {
        public int ItemId { get; set; }
        public string? Message { get; set; }
        public string? ConfirmationAction { get; set; }
        public string? ConfirmationController { get; set; }
    }
}
