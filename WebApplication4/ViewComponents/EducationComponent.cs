using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class EducationComponent : ViewComponent
    {
        private readonly MyPortfolioContext _context;

        public EducationComponent(MyPortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var educations = _context.Educations.ToList();
            return View(educations);
        }
    }
}