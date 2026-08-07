using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly MyPortfolioContext _context;

        public PortfolioController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var portfolios = _context.Portfolios.ToList();
            return View(portfolios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Portfolio model)
        {
            _context.Portfolios.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var portfolio = _context.Portfolios.FirstOrDefault(p => p.PortfolioId == id);
            if (portfolio != null)
            {
                _context.Portfolios.Remove(portfolio);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}