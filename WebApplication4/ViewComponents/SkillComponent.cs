using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class SkillComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }


    }
}
