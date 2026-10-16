using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly MyPortfolioContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(MyPortfolioContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var portfolios = _context.Portfolios.ToList();
            return View(portfolios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Portfolio model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var izinliUzantilar = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var uzanti = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (!izinliUzantilar.Contains(uzanti))
                {
                    ModelState.AddModelError("", "Sadece jpg, png veya webp yükleyebilirsiniz.");
                    return View(model);
                }

                if (ImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Dosya boyutu 5MB'ı geçemez.");
                    return View(model);
                }

                var dosyaAdi = $"{Guid.NewGuid()}{uzanti}";
                var klasorYolu = Path.Combine(_env.WebRootPath, "uploads", "portfolio");

                if (!Directory.Exists(klasorYolu))
                {
                    Directory.CreateDirectory(klasorYolu);
                }

                var tamYol = Path.Combine(klasorYolu, dosyaAdi);

                // Kopyalama işlemi asenkron yapıldı
                using (var stream = new FileStream(tamYol, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                model.ImageURL = $"/uploads/portfolio/{dosyaAdi}";
            }

            _context.Portfolios.Add(model);
            // Veritabanı kaydı asenkron yapıldı
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var portfolio = _context.Portfolios.FirstOrDefault(p => p.PortfolioId == id);
            if (portfolio != null)
            {
                if (!string.IsNullOrEmpty(portfolio.ImageURL))
                {
                    var dosyaYolu = Path.Combine(_env.WebRootPath, portfolio.ImageURL.TrimStart('/'));
                    if (System.IO.File.Exists(dosyaYolu))
                    {
                        System.IO.File.Delete(dosyaYolu);
                    }
                }

                _context.Portfolios.Remove(portfolio);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
