using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    public class ContactController : Controller
    {
        private readonly MyPortfolioContext _context;
        private readonly ILogger<ContactController> _logger;

        public ContactController(MyPortfolioContext context, ILogger<ContactController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var contact = _context.Contacts.FirstOrDefault();
            if (contact == null)
            {
                contact = new Contact { Title = "", Description = "", Phone1 = "", Email1 = "", Address = "" };
            }
            return View(contact);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = _context.Contacts.FirstOrDefault();

            if (existing == null)
            {
                _context.Contacts.Add(model);
            }
            else
            {
                existing.Title = model.Title;
                existing.Description = model.Description;
                existing.Phone1 = model.Phone1;
                existing.Email1 = model.Email1;
                existing.Address = model.Address;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Bu action anonim (giriş yapmamış) ziyaretçiler tarafından kullanılır,
        // bu yüzden global CSRF token zorunluluğundan hariç tutuluyor.
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [EnableRateLimiting("contact")]
        public IActionResult SendMessage(string? contactName, string? contactSubject, string? contactEmail, string? contactMessage)
        {
            var message = new Message
            {
                NameSurname = contactName?.Trim() ?? "",
                Subject = string.IsNullOrWhiteSpace(contactSubject) ? "(Konu yok)" : contactSubject.Trim(),
                Email = contactEmail?.Trim() ?? "",
                MessageDetail = contactMessage?.Trim() ?? "",
                SendDate = DateTime.UtcNow,
                IsRead = false
            };

            TempData["ContactSuccess"] = TrySave(message);
            return Redirect("/#contact");
        }

        // Bu action da anonim ziyaretçiler tarafından kullanılır (Hakkımda bölümündeki hızlı form).
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [EnableRateLimiting("contact")]
        public IActionResult SendQuickMessage(string? contactEmail, string? contactMessage)
        {
            var message = new Message
            {
                NameSurname = "Bilinmiyor",
                Subject = "Hızlı Mesaj (Hakkımda formu)",
                Email = contactEmail?.Trim() ?? "",
                MessageDetail = contactMessage?.Trim() ?? "",
                SendDate = DateTime.UtcNow,
                IsRead = false
            };

            TempData["QuickSuccess"] = TrySave(message);
            return Redirect("/#about");
        }

        // Parametreler ayrı ayrı bağlandığı için Message entity'sinin [Required]/[EmailAddress]
        // kurallarını elle tetikliyoruz. Veritabanı hatası olursa ziyaretçiye 500 sayfası yerine
        // "gönderilemedi" mesajı gösterilir.
        private bool TrySave(Message message)
        {
            // Nullable açıkken string parametreler otomatik "zorunlu" sayılır; isteğe bağlı
            // Konu alanı boş geldiğinde tüm doğrulama düşüyordu. Parametre hatalarını temizleyip
            // yalnızca Message entity'sinin kurallarına göre doğruluyoruz.
            ModelState.Clear();
            if (!TryValidateModel(message))
            {
                return false;
            }

            try
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İletişim mesajı kaydedilemedi.");
                return false;
            }
        }
    }
}