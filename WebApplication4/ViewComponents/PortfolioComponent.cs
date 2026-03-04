using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class PortfolioComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
