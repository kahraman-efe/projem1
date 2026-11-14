using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    // Erden Yazılım hizmetleri bölümü. İçerik sabittir, veritabanı kullanmaz.
    public class ServicesComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
