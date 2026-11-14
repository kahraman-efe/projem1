using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.ViewComponents
{
    public class AboutComponent : SafeViewComponent
    {
        public AboutComponent(MyPortfolioContext context) : base(context) { }

        public IViewComponentResult Invoke()
        {
            var item = Safe(db => db.Abouts.FirstOrDefault(), null);
            return View(item);
        }
    }
}
