using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.ViewComponents
{
    public class HeadComponent : ViewComponent
    {
        // Alt sayfalar (ör. KVKK) kendi başlık/açıklama/adresini verebilir; verilmezse ana sayfa değerleri kullanılır.
        public IViewComponentResult Invoke(string? title = null, string? description = null, string? path = null)
        {
            ViewData["HeadTitle"] = title ?? "Kahraman Efe | Yazılım Geliştirici";
            ViewData["HeadDescription"] = description ?? "Kahraman Efe'nin kişisel portföy sitesi. Yazılım geliştirici olarak projeler, deneyimler ve iletişim bilgileri.";
            ViewData["HeadUrl"] = "https://kahramanefe.com.tr" + (path ?? "");
            return View();
        }
    }
}
