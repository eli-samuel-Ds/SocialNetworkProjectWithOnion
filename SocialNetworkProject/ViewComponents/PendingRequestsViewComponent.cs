using Microsoft.AspNetCore.Mvc;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Helpers;

namespace SocialNetworkProject.ViewComponents
{
    public class PendingRequestsViewComponent : ViewComponent
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PendingRequestsViewComponent(IFriendshipService friendshipService, IHttpContextAccessor httpContextAccessor)
        {
            _friendshipService = friendshipService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user");
            int count = 0;
            if (user != null)
            {
                count = await _friendshipService.GetPendingRequestCountAsync(user.Id);
            }
            return View(count);
        }
    }
}
