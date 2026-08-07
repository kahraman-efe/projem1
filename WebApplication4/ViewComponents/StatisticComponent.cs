using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class StatisticComponent : ViewComponent
    {
        private readonly MyPortfolioContext _context;

        public StatisticComponent(MyPortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var statistics = _context.Statistics.ToList();
            return View(statistics);
        }
    }
}