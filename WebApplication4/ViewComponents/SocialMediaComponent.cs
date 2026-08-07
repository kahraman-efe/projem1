using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class SocialMediaComponent : ViewComponent
    {
        private readonly MyPortfolioContext _context;

        public SocialMediaComponent(MyPortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var socialMedias = _context.SocialMedias.ToList();
            return View(socialMedias);
        }
    }
}