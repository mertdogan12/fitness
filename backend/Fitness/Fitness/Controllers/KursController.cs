using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
{
    public class KursController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
