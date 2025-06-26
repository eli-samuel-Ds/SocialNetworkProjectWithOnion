using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Helpers;

namespace SocialNetworkProject.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userSession = context.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (userSession != null)
            {
                var controller = (Controller)context.Controller;
                context.Result = controller.RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                await next();
            }
        }
    }
}
