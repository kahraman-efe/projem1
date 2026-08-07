using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    public class ExperienceController : Controller
    {
        private readonly MyPortfolioContext _context;

        public ExperienceController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult ExperienceList()
        {
            return View();
        }

        [Authorize]
        public IActionResult Index()
        {
            var experiences = _context.Experiences.ToList();
            return View(experiences);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Experience model)
        {
            _context.Experiences.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var experience = _context.Experiences.FirstOrDefault(e => e.ExperienceID == id);
            if (experience != null)
            {
                _context.Experiences.Remove(experience);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}