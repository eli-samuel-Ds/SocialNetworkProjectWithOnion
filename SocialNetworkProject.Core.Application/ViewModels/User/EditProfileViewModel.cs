using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.User
{
    public class EditProfileViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe colocar su nombre.")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Debe colocar su apellido.")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Debe colocar un número de teléfono.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "El formato del teléfono debe ser 809-555-5555.")]
        public required string PhoneNumber { get; set; }

        public string? ProfilePictureUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ProfilePictureFile { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmPassword { get; set; }
    }
}
