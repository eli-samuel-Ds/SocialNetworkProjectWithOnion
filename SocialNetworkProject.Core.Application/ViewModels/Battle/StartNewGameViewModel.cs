using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class StartNewGameViewModel
    {
        public string? SearchTerm { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un oponente.")]
        public string? SelectedOpponentId { get; set; }
        public List<PotentialOpponentViewModel> AvailableFriends { get; set; } = new();
    }
}
