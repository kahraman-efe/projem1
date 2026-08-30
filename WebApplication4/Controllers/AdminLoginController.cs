using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    public class AdminLoginController : Controller
    {
        private readonly MyPortfolioContext _context;

        // ASP.NET Core Identity'nin PasswordHasher'ı: PBKDF2 tabanlı, her şifre için
        // otomatik rastgele salt üretir ve kaba kuvvet (brute-force) saldırılarına karşı
        // SHA256'dan çok daha dayanıklıdır. Framework'ün bir parçası, ekstra paket gerekmez.
        private readonly PasswordHasher<Admin> _passwordHasher = new PasswordHasher<Admin>();

        public AdminLoginController(MyPortfolioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Sadece veritabanında HİÇ admin kaydı yokken erişilebilir. İlk admin hesabı
        // oluşturulduktan sonra bu sayfa otomatik olarak kapanır (aşağıdaki kontrol sayesinde),
        // bu yüzden başkası tarafından ikinci bir "gizli" admin oluşturmak için kullanılamaz.
        [HttpGet]
        public IActionResult Setup()
        {
            if (_context.Admins.Any())
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Setup(string username, string password, string confirmPassword)
        {
            if (_context.Admins.Any())
            {
                return RedirectToAction("Login");
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Kullanıcı adı ve şifre zorunludur.";
                return View();
            }

            if (password.Length < 8)
            {
                ViewBag.Error = "Şifre en az 8 karakter olmalıdır.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor.";
                return View();
            }

            var admin = new Admin { Username = username.Trim() };
            admin.PasswordHash = _passwordHasher.HashPassword(admin, password);

            _context.Admins.Add(admin);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Username == username);

            if (admin == null || !VerifyPassword(admin, password))
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Şifreyi doğrular. Yeni PasswordHasher formatını dener; eğer kayıt hâlâ eski
        // SHA256 formatındaysa (bu projenin önceki sürümünden kalma), onu da kontrol eder
        // ve doğruysa veritabanındaki hash'i otomatik olarak güvenli formata yükseltir.
        // Böylece mevcut admin hesabının şifresini sıfırlamaya gerek kalmaz.
        private bool VerifyPassword(Admin admin, string password)
        {
            if (string.IsNullOrEmpty(admin.PasswordHash))
            {
                return false;
            }

            // Eski SHA256 hash'leri her zaman 64 karakter hex string'dir.
            // Yeni PasswordHasher çıktısı ise Base64 formatındadır ve farklı uzunluktadır.
            bool looksLegacy = admin.PasswordHash.Length == 64 && admin.PasswordHash.All(Uri.IsHexDigit);

            if (looksLegacy)
            {
                if (LegacySha256Hash(password) != admin.PasswordHash)
                {
                    return false;
                }

                // Doğru şifre girildi: hash'i güvenli formata yükselt ve kaydet.
                admin.PasswordHash = _passwordHasher.HashPassword(admin, password);
                _context.SaveChanges();
                return true;
            }

            var result = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                admin.PasswordHash = _passwordHasher.HashPassword(admin, password);
                _context.SaveChanges();
            }

            return true;
        }

        // Sadece eski (migration öncesi) kayıtları doğrulamak için tutuluyor.
        // Yeni şifreler artık bu yöntemle hash'lenmiyor.
        private string LegacySha256Hash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}