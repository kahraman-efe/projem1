using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    public class ExperienceController : Controller
    {
        public IActionResult ExperienceList()
        {
            return View();
        }
    }
}
