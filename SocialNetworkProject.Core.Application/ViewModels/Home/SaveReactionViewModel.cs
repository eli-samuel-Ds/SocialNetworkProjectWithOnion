using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.ViewModels.Home
{
    public class SaveReactionViewModel
    {
        public int PostId { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}
