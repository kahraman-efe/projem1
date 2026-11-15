using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private static readonly string[] IzinliUzantilar = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaksDosyaBoyutu = 5 * 1024 * 1024;

        private readonly MyPortfolioContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(MyPortfolioContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var portfolios = _context.Portfolios.OrderByDescending(p => p.PortfolioId).ToList();
            return View(portfolios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Portfolio { Status = ProjectStatus.Tamamlandi });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Portfolio model, IFormFile? ImageFile)
        {
            PrepareAndValidate(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var imageUrl = await SaveImageAsync(ImageFile);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ImageURL = imageUrl ?? "";
            _context.Portfolios.Add(model);
            await _context.SaveChangesAsync();

            TempData["AdminSuccess"] = "Proje eklendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var portfolio = _context.Portfolios.FirstOrDefault(p => p.PortfolioId == id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View("Create", portfolio);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Portfolio model, IFormFile? ImageFile)
        {
            var existing = _context.Portfolios.FirstOrDefault(p => p.PortfolioId == id);
            if (existing == null)
            {
                return NotFound();
            }

            model.PortfolioId = id;
            model.ImageURL = existing.ImageURL; // mevcut görsel formda gösterilsin
            PrepareAndValidate(model);
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            var newImageUrl = await SaveImageAsync(ImageFile);
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            existing.Title = model.Title;
            existing.Subtitle = model.Subtitle;
            existing.URL = model.URL;
            existing.Description = model.Description;
            existing.Status = model.Status;

            if (newImageUrl != null)
            {
                DeleteImageFile(existing.ImageURL);
                existing.ImageURL = newImageUrl;
            }

            await _context.SaveChangesAsync();

            TempData["AdminSuccess"] = "Proje güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var portfolio = _context.Portfolios.FirstOrDefault(p => p.PortfolioId == id);
            if (portfolio != null)
            {
                DeleteImageFile(portfolio.ImageURL);
                _context.Portfolios.Remove(portfolio);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Boş bırakılabilen alanları temizler, linki normalleştirip doğrular.
        private void PrepareAndValidate(Portfolio model)
        {
            // Nullable açıkken string alanlar otomatik "zorunlu" sayılıyor; bunlar isteğe bağlı.
            foreach (var key in new[] { "Subtitle", "URL", "Description", "ImageURL" })
            {
                ModelState.Remove(key);
            }

            model.Title = model.Title?.Trim() ?? "";
            model.Subtitle = model.Subtitle?.Trim() ?? "";
            model.Description = model.Description?.Trim() ?? "";

            var url = model.URL?.Trim() ?? "";
            if (url.Length > 0)
            {
                // "siteadi.com", "www.siteadi.com.tr" gibi adresleri https:// ile tamamla
                if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    url = "https://" + url;
                }

                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
                    !uri.Host.Contains('.'))
                {
                    ModelState.AddModelError("URL", "Proje linki geçerli bir web adresi olmalı (ör. siteadi.com veya https://github.com/...).");
                }
            }
            model.URL = url;
        }

        // Görsel yüklendiyse kaydeder ve adresini döner; yüklenmediyse null.
        private async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var uzanti = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!IzinliUzantilar.Contains(uzanti))
            {
                ModelState.AddModelError("", "Sadece jpg, png veya webp yükleyebilirsiniz.");
                return null;
            }

            if (imageFile.Length > MaksDosyaBoyutu)
            {
                ModelState.AddModelError("", "Dosya boyutu 5MB'ı geçemez.");
                return null;
            }

            var klasorYolu = Path.Combine(_env.WebRootPath, "uploads", "portfolio");
            Directory.CreateDirectory(klasorYolu);

            var dosyaAdi = $"{Guid.NewGuid()}{uzanti}";
            using (var stream = new FileStream(Path.Combine(klasorYolu, dosyaAdi), FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/portfolio/{dosyaAdi}";
        }

        private void DeleteImageFile(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith("/uploads/"))
            {
                return;
            }

            var dosyaYolu = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(dosyaYolu))
            {
                System.IO.File.Delete(dosyaYolu);
            }
        }
    }
}
