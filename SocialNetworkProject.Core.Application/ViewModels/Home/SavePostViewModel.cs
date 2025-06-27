using Microsoft.AspNetCore.Http;
using SocialNetworkProject.Core.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.Home
{
    public class SavePostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El contenido de la publicación es obligatorio.")]
        [DataType(DataType.Text)]
        public string Content { get; set; }

        public MediaType MediaType { get; set; } = MediaType.None;

        public IFormFile? ImageFile { get; set; }

        [DataType(DataType.Url)]
        public string? VideoUrl { get; set; }

        public string? ExistingMediaUrl { get; set; }
    }
}
