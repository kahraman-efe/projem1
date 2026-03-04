using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class FeatureComponent : ViewComponent
    {
        MyPortfolioContext portfolioContext=new MyPortfolioContext();
        public IViewComponentResult Invoke()
        {
            var values=portfolioContext.Features.ToList();
            return View(values);
        }

    }
}
