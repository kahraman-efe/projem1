using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using System.Security.Cryptography;

using System.Text;

using WebApplication4.DAL.Context;

namespace WebApplication4.Controllers

{

    public class AdminLoginController : Controller

    {

        private readonly MyPortfolioContext _context;

        public AdminLoginController(MyPortfolioContext context)

        {

            _context = context;

        }

        [HttpGet]

        public IActionResult Login()

        {

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Login(string username, string password)

        {

            string hashedInput = HashPassword(password);

            var admin = _context.Admins.FirstOrDefault(a =>

                a.Username == username && a.PasswordHash == hashedInput);

            if (admin == null)

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

        private string HashPassword(string password)

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