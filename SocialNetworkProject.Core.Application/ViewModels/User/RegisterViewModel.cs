using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Debe colocar su nombre.")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Debe colocar su apellido.")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Debe colocar un correo electrónico.")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar un número de teléfono.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "El formato del teléfono debe ser 809-555-5555.")]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de usuario.")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una foto de perfil.")]
        [DataType(DataType.Upload)]
        public required IFormFile ProfilePictureFile { get; set; }
    }
}
