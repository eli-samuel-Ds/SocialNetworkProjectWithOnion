using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }

        [Required(ErrorMessage = "Debe colocar una nueva contraseña.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        [Required(ErrorMessage = "Debe confirmar la nueva contraseña.")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
}
