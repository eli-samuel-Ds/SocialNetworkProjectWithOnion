using Microsoft.AspNetCore.Mvc;

namespace SocialNetworkProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
