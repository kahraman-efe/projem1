using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        private readonly MyPortfolioContext _context;

        public StatisticController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var statistics = _context.Statistics.ToList();
            return View(statistics);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Statistic model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Statistics.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var statistic = _context.Statistics.FirstOrDefault(s => s.StatisticId == id);
            if (statistic != null)
            {
                _context.Statistics.Remove(statistic);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}