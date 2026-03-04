using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class TestimonialComponent:ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
