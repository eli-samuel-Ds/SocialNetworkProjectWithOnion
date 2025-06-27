using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProject.Core.Application.ViewModels.Home
{
    public class SaveCommentViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "El texto del comentario no puede estar vacío.")]
        public string Text { get; set; }

        public int? ParentCommentId { get; set; } 
    }
}
