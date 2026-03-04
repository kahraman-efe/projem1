using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class HeadComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
