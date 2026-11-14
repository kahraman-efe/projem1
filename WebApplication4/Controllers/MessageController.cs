using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly MyPortfolioContext _context;
        private readonly ILogger<MessageController> _logger;

        public MessageController(MyPortfolioContext context, ILogger<MessageController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages.OrderByDescending(m => m.SendDate).ToList();

            // Bu ziyarette "Yeni" olarak gösterilecekleri işaretle, ardından okundu olarak kaydet.
            var newIds = messages.Where(m => !m.IsRead).Select(m => m.MessageID).ToHashSet();
            ViewBag.NewIds = newIds;

            if (newIds.Count > 0)
            {
                foreach (var m in messages.Where(m => newIds.Contains(m.MessageID)))
                {
                    m.IsRead = true;
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Mesajlar okundu olarak işaretlenemedi.");
                }
            }

            return View(messages);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = _context.Messages.Find(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
