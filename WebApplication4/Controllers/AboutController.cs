using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class AboutController : Controller
    {
        private readonly MyPortfolioContext _context;

        public AboutController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var about = _context.Abouts.FirstOrDefault();

            if (about == null)
            {
                about = new About { Title = "", SubDescription = "", Details = "" };
            }

            return View(about);
        }

        [HttpPost]
        public IActionResult Index(About model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = _context.Abouts.FirstOrDefault();

            if (existing == null)
            {
                _context.Abouts.Add(model);
            }
            else
            {
                existing.Title = model.Title;
                existing.SubDescription = model.SubDescription;
                existing.Details = model.Details;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}