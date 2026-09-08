using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class FeatureController : Controller
    {
        private readonly MyPortfolioContext _context;

        public FeatureController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var features = _context.Features.ToList();
            return View(features);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Feature model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Features.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var feature = _context.Features.FirstOrDefault(f => f.FeatureId == id);
            if (feature != null)
            {
                _context.Features.Remove(feature);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}