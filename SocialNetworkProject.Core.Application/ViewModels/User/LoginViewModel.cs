using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe colocar su nombre de usuario.")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Debe colocar su contraseña.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
