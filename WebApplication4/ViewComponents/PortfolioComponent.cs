using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.ViewComponents
{
    public class PortfolioComponent : SafeViewComponent
    {
        public PortfolioComponent(MyPortfolioContext context) : base(context) { }

        public IViewComponentResult Invoke()
        {
            var items = Safe(db => db.Portfolios.ToList(), new List<Portfolio>());
            return View(items);
        }
    }
}
