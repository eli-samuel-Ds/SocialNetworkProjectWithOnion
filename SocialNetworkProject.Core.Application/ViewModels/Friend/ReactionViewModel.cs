using SocialNetworkProject.Core.Domain.Common.Enums;

namespace SocialNetworkProject.Core.Application.ViewModels.Friend
{
    public class ReactionViewModel
    {
        public ReactionType Reaction { get; set; }
        public int Count { get; set; }
        public bool UserHasReacted { get; set; } 
    }
}
