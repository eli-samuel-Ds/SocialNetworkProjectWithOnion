using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.User
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Debe colocar su nombre de usuario.")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }
    }
}
