using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class FeatureComponent : ViewComponent
    {
        private readonly MyPortfolioContext portfolioContext;

        public FeatureComponent(MyPortfolioContext portfolioContext)
        {
            this.portfolioContext = portfolioContext;
        }

        public IViewComponentResult Invoke()
        {
            var values = portfolioContext.Features.ToList();
            return View(values);
        }
    }
}
