using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.ViewComponents
{
    public class TestimonialComponent : SafeViewComponent
    {
        public TestimonialComponent(MyPortfolioContext context) : base(context) { }

        public IViewComponentResult Invoke()
        {
            var items = Safe(db => db.Testimonials.ToList(), new List<Testimonial>());
            return View(items);
        }
    }
}
