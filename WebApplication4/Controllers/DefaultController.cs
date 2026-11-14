using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.Controllers
{
    public class DefaultController : Controller
    {
        private readonly MyPortfolioContext _context;
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(MyPortfolioContext context, ILogger<DefaultController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // KVKK Aydınlatma Metni, Gizlilik ve Çerez Politikası
        [HttpGet("/kvkk")]
        public IActionResult Kvkk()
        {
            // Başvuru adresi olarak admin panelinden girilen iletişim e-postası kullanılır.
            string? email = null;
            try
            {
                email = _context.Contacts.Select(c => c.Email1).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "KVKK sayfası için iletişim e-postası okunamadı.");
            }

            ViewBag.Email = string.IsNullOrWhiteSpace(email) ? null : email;
            return View();
        }
    }
}
