using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly MyPortfolioContext _context;

        public MessageController(MyPortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages.OrderByDescending(m => m.SendDate).ToList();
            return View(messages);
        }
    }
}