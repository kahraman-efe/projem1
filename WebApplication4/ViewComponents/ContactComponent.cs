using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class ContactComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
