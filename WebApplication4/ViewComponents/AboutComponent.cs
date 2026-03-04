using Microsoft.AspNetCore.Mvc;


namespace WebApplication4.ViewComponents
{
    public class AboutComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
