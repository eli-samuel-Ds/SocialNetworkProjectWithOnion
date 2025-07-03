using SocialNetworkProject.Core.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.Battle
{
    public class PlaceShipViewModel
    {
        [Required]
        public int BattleId { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un barco de la lista.")]
        public int? SelectedShipId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una celda de inicio en el tablero.")]
        public int? StartX { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una celda de inicio en el tablero.")]
        public int? StartY { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una dirección para posicionar el barco.")]
        public ShipDirection? Direction { get; set; }
    }
}
