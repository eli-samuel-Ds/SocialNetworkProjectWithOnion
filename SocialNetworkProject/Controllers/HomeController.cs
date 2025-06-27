using Microsoft.AspNetCore.Mvc;
using SocialNetworkProject.Middlewares;

namespace SocialNetworkProject.Controllers
{
    [ServiceFilter(typeof(UserAuthorize))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
