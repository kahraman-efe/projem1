using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.ViewComponents
{
    public class ContactComponent : SafeViewComponent
    {
        public ContactComponent(MyPortfolioContext context) : base(context) { }

        public IViewComponentResult Invoke()
        {
            var item = Safe(db => db.Contacts.FirstOrDefault(), null);
            return View(item);
        }
    }
}
