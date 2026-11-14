using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.ViewComponents
{
    public class ExperienceComponent : SafeViewComponent
    {
        public ExperienceComponent(MyPortfolioContext context) : base(context) { }

        public IViewComponentResult Invoke()
        {
            var items = Safe(db => db.Experiences.ToList(), new List<Experience>());
            return View(items);
        }
    }
}
