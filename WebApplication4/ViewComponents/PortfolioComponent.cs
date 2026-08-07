using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class PortfolioComponent : ViewComponent
    {
        private readonly MyPortfolioContext _context;

        public PortfolioComponent(MyPortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var portfolios = _context.Portfolios.ToList();
            return View(portfolios);
        }
    }
}