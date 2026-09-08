using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class TestimonialController : Controller
    {
        private readonly MyPortfolioContext _context;

        public TestimonialController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var testimonials = _context.Testimonials.ToList();
            return View(testimonials);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Testimonial model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Testimonials.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var testimonial = _context.Testimonials.FirstOrDefault(t => t.TestimonialId == id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}