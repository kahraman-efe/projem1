using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.ViewComponents
{
    public class FeatureComponent : SafeViewComponent
    {
        public FeatureComponent(MyPortfolioContext context) : base(context) { }

        public IViewComponentResult Invoke()
        {
            var items = Safe(db => db.Features.ToList(), new List<Feature>());
            return View(items);
        }
    }
}
