using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;

namespace WebApplication4.ViewComponents
{
    /// <summary>
    /// Ana sayfadaki tüm bölümlerin ortak temeli.
    /// Veritabanına ulaşılamazsa (bağlantı hatası, duraklatılmış sunucu vb.) tüm sayfanın
    /// 500 hatası vermesi yerine o bölüm boş veriyle çizilir ve hata loglanır.
    /// </summary>
    public abstract class SafeViewComponent : ViewComponent
    {
        protected readonly MyPortfolioContext Context;

        protected SafeViewComponent(MyPortfolioContext context)
        {
            Context = context;
        }

        private const string DbDownKey = "__PortfolioDbUnavailable";

        protected T Safe<T>(Func<MyPortfolioContext, T> query, T fallback)
        {
            // Aynı istekte veritabanı bir kez hata verdiyse diğer bölümler tekrar
            // bağlanmaya çalışıp (zaman aşımı kadar) sayfayı yavaşlatmasın.
            if (HttpContext.Items.ContainsKey(DbDownKey))
            {
                return fallback;
            }

            try
            {
                return query(Context);
            }
            catch (Exception ex)
            {
                HttpContext.Items[DbDownKey] = true;
                var logger = HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger(GetType());
                logger.LogError(ex, "{Component} verisi okunamadı; bölüm boş gösterilecek.", GetType().Name);
                return fallback;
            }
        }
    }
}
