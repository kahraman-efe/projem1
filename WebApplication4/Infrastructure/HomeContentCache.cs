using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace WebApplication4.Infrastructure
{
    /// <summary>
    /// Ana sayfa bölümlerinin veritabanı sonuçlarını bellekte tutar.
    /// Veritabanı uzak bir bölgede olduğu için her ziyarette 10 ayrı sorgu sayfayı yavaşlatıyordu.
    /// Admin panelinden herhangi bir kayıt değiştiğinde önbellek hemen temizlenir (bkz. ClearHomeCacheOnAdminWriteFilter).
    /// </summary>
    public class HomeContentCache
    {
        private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(10);

        private readonly IMemoryCache _cache;
        private CancellationTokenSource _reset = new();
        private readonly object _lock = new();

        public HomeContentCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (_cache.TryGetValue(key, out var cached) && cached is T typed)
            {
                value = typed;
                return true;
            }
            value = default!;
            return false;
        }

        public void Set<T>(string key, T value)
        {
            CancellationToken token;
            lock (_lock)
            {
                token = _reset.Token;
            }

            _cache.Set(key, value, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(Lifetime)
                .AddExpirationToken(new CancellationChangeToken(token)));
        }

        public void Clear()
        {
            CancellationTokenSource old;
            lock (_lock)
            {
                old = _reset;
                _reset = new CancellationTokenSource();
            }
            old.Cancel();
            old.Dispose();
        }
    }

    /// <summary>
    /// Giriş yapmış yöneticinin yaptığı her POST isteğinden sonra (ekle/düzenle/sil)
    /// ana sayfa önbelleğini temizler; değişiklikler sitede anında görünür.
    /// </summary>
    public class ClearHomeCacheOnAdminWriteFilter : IAsyncActionFilter
    {
        private readonly HomeContentCache _cache;

        public ClearHomeCacheOnAdminWriteFilter(HomeContentCache cache)
        {
            _cache = cache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();

            var http = context.HttpContext;
            if (HttpMethods.IsPost(http.Request.Method) && http.User.Identity?.IsAuthenticated == true)
            {
                _cache.Clear();
            }
        }
    }
}
