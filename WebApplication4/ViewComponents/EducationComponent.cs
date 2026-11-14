using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.ViewComponents
{
    public class EducationComponent : SafeViewComponent
    {
        public EducationComponent(MyPortfolioContext context) : base(context) { }

        public IViewComponentResult Invoke()
        {
            var items = Safe(db => db.Educations.ToList(), new List<Education>());
            return View(items);
        }
    }
}
