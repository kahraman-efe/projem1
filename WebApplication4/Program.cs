using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL.Context;
using WebApplication4.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// AutoValidateAntiforgeryTokenAttribute: siteye ait TÜM POST/PUT/DELETE isteklerinde
// otomatik olarak anti-forgery (CSRF) token doğrulaması yapar. Tek tek her controller'a
// [ValidateAntiForgeryToken] eklemeyi unutma riskini ortadan kaldırır.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    // Admin panelinden yapılan her kayıttan sonra ana sayfa önbelleğini temizler.
    options.Filters.Add<ClearHomeCacheOnAdminWriteFilter>();
});

// Ana sayfa verileri için bellek önbelleği (bkz. Infrastructure/HomeContentCache.cs)
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<HomeContentCache>();
builder.Services.AddScoped<ClearHomeCacheOnAdminWriteFilter>();

// Site Render + Cloudflare arkasında çalışıyor. Bu ayar sayesinde uygulama isteğin aslında
// HTTPS ile geldiğini anlar (HSTS başlığı ancak böyle gönderilir).
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var connectionString = builder.Configuration.GetConnectionString("MyPortfolioDb");
builder.Services.AddDbContext<MyPortfolioContext>(options =>
    options.UseNpgsql(connectionString)); 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/AdminLogin/Login";
    });

// Kaba kuvvet (brute-force) ve spam koruması: aynı IP adresinden gelen istekleri sınırlar.
// "login": giriş/kurulum formu için dakikada 5 deneme. "contact": iletişim formları için dakikada 5 mesaj.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, token) =>
    {
        ctx.HttpContext.Response.ContentType = "text/html; charset=utf-8";
        await ctx.HttpContext.Response.WriteAsync(
            "<!DOCTYPE html><html lang=\"tr\"><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\">" +
            "<title>Çok fazla istek</title><body style=\"font-family:system-ui,sans-serif;background:#111;color:#eee;display:grid;place-items:center;min-height:100vh;margin:0;text-align:center;padding:16px\">" +
            "<div><h1 style=\"font-size:1.6rem\">Çok fazla deneme yaptınız</h1><p>Lütfen bir dakika bekleyip tekrar deneyin.</p>" +
            "<p><a style=\"color:#d43b2f\" href=\"javascript:history.back()\">Geri dön</a></p></div></body></html>", token);
    };
    options.AddPolicy("login", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        ClientIp.Get(httpContext),
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
    options.AddPolicy("contact", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        ClientIp.Get(httpContext),
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Tarayıcı güvenlik başlıkları (clickjacking, MIME sniffing, gereksiz izinler, dış kaynak kısıtlaması)
app.Use(async (context, next) =>
{
    var h = context.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["X-Frame-Options"] = "SAMEORIGIN";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    h["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=(), usb=()";
    h["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline'; " +
        "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://fonts.googleapis.com; " +
        "font-src 'self' data: https://cdn.jsdelivr.net https://fonts.gstatic.com; " +
        "img-src 'self' data: https:; " +
        "connect-src 'self'; " +
        "object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'self'";
    await next();
});

app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");
app.Run();