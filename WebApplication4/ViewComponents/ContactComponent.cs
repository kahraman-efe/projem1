using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    public class ContactComponent : ViewComponent
    {
        private readonly MyPortfolioContext _context;

        public ContactComponent(MyPortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var contact = _context.Contacts.FirstOrDefault();
            return View(contact);
        }
    }
}