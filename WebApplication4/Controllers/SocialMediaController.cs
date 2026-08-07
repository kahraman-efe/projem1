using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class SocialMediaController : Controller
    {
        private readonly MyPortfolioContext _context;

        public SocialMediaController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var socialMedias = _context.SocialMedias.ToList();
            return View(socialMedias);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SocialMedia model)
        {
            _context.SocialMedias.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var socialMedia = _context.SocialMedias.FirstOrDefault(s => s.SocialMediaId == id);
            if (socialMedia != null)
            {
                _context.SocialMedias.Remove(socialMedia);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}