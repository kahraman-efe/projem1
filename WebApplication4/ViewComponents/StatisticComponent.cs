using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class StatisticComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }



    }
}
