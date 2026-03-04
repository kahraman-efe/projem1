using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
