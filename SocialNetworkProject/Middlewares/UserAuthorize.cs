using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Helpers;

namespace SocialNetworkProject.Middlewares
{
    public class UserAuthorize : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userSession = context.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (userSession == null)
            {
                var controller = (Controller)context.Controller;
                context.Result = controller.RedirectToRoute(new { controller = "User", action = "Index" });
            }
            else
            {
                await next();
            }
        }
    }
}
