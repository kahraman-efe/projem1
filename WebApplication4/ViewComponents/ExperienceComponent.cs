using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class ExperienceComponent : ViewComponent
    {
        private readonly MyPortfolioContext _context;

        public ExperienceComponent(MyPortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var experiences = _context.Experiences.ToList();
            return View(experiences);
        }
    }
}