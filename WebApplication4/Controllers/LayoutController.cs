using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
