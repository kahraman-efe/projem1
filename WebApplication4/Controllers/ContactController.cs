using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    public class ContactController : Controller
    {
        private readonly MyPortfolioContext _context;

        public ContactController(MyPortfolioContext context)
        {
            _context = context;
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

        [HttpPost]
        public IActionResult SendMessage(string contactName, string contactSubject, string contactEmail, string contactMessage)
        {
            var message = new Message
            {
                NameSurname = contactName,
                Subject = contactSubject,
                Email = contactEmail,
                MessageDetail = contactMessage,
                SendDate = DateTime.Now,
                IsRead = false
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            TempData["ContactSuccess"] = true;
            return Redirect("/Default/Index#contact");
        }

        [HttpPost]
        public IActionResult SendQuickMessage(string contactEmail, string contactMessage)
        {
            var message = new Message
            {
                NameSurname = "Bilinmiyor",
                Subject = "Hızlı Mesaj (About formu)",
                Email = contactEmail,
                MessageDetail = contactMessage,
                SendDate = DateTime.Now,
                IsRead = false
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            return Redirect("/Default/Index#about");
        }
    }
}