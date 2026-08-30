using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

// AutoValidateAntiforgeryTokenAttribute: siteye ait TÜM POST/PUT/DELETE isteklerinde
// otomatik olarak anti-forgery (CSRF) token doğrulaması yapar. Tek tek her controller'a
// [ValidateAntiForgeryToken] eklemeyi unutma riskini ortadan kaldırır.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

var connectionString = builder.Configuration.GetConnectionString("MyPortfolioDb");
builder.Services.AddDbContext<MyPortfolioContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/AdminLogin/Login";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();