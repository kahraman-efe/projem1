using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    public class EducationController : Controller
    {
        private readonly MyPortfolioContext _context;

        public EducationController(MyPortfolioContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var educations = _context.Educations.ToList();
            return View(educations);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Education model)
        {
            _context.Educations.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var education = _context.Educations.FirstOrDefault(e => e.EducationId == id);
            if (education != null)
            {
                _context.Educations.Remove(education);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}