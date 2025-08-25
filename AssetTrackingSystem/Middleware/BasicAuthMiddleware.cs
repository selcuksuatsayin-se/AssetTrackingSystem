using Microsoft.Extensions.Primitives;

namespace AssetTrackingSystem.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<(string Username, string Password)> _validUsers;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
            _validUsers = new List<(string, string)>
            {
                ("admin1", "sifre1"),
                ("admin2", "sifre2"),
                ("admin3", "sifre3"),
                ("admin4", "sifre4"),
                ("selcuk", "selcuk"),
                ("tayfun.uye", "155155")

            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Login sayfasını ve statik dosyaları bypass et
            if (context.Request.Path.StartsWithSegments("/Auth/Login") ||
                context.Request.Path.StartsWithSegments("/login") ||
                context.Request.Path.StartsWithSegments("/lib") ||
                context.Request.Path.StartsWithSegments("/css") ||
                context.Request.Path.StartsWithSegments("/js") ||
                context.Request.Path.StartsWithSegments("/favicon.ico"))
            {
                await _next(context);
                return;
            }

            // Session'da auth kontrolü
            if (context.Session.GetString("IsAuthenticated") == "true")
            {
                await _next(context);
                return;
            }

            // Login sayfasına yönlendir
            context.Response.Redirect("/Auth/Login");
        }
    }
}