using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.Friendship
{
    public class AddFriendViewModel
    {
        public string? SearchTerm { get; set; }
        public List<PotentialFriendViewModel> PotentialFriends { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un usuario para enviar la solicitud.")]
        public string? SelectedUserId { get; set; }

        public AddFriendViewModel()
        {
            PotentialFriends = new List<PotentialFriendViewModel>();
        }
    }
}
