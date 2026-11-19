using System.Net;

namespace WebApplication4.Infrastructure
{
    /// <summary>
    /// Ziyaretçinin gerçek IP adresini bulur. Site Cloudflare + Render arkasında olduğu için
    /// bağlantı IP'si her zaman proxy'nin adresidir; gerçek adres başlıklarda gelir.
    /// Rate limit (dakikada 5 mesaj/giriş) bu adrese göre kişi başı uygulanır.
    /// </summary>
    public static class ClientIp
    {
        public static string Get(HttpContext context)
        {
            // Cloudflare'in eklediği başlık
            var cf = context.Request.Headers["CF-Connecting-IP"].ToString();
            if (IPAddress.TryParse(cf, out _))
            {
                return cf;
            }

            // Render vb. proxy'lerin eklediği zincir: "istemci, proxy1, proxy2"
            var xff = context.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrWhiteSpace(xff))
            {
                var first = xff.Split(',')[0].Trim();
                if (IPAddress.TryParse(first, out _))
                {
                    return first;
                }
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}
