using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class SkillController : Controller
    {
        private readonly MyPortfolioContext _context;

        public SkillController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var skills = _context.Skills.ToList();
            return View(skills);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Skill model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Skills.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.SkillId == id);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}