using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class ExperienceComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
