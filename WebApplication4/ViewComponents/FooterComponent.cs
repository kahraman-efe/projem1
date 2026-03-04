using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class FooterComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
