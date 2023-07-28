using COMMON;
using COMMON.APP_SETTINGS;
using Microsoft.Extensions.Options;

namespace FAMILY_TREE.MIDDLEWARES
{
    public class AppsettingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appsettings;
        public AppsettingMiddleware(RequestDelegate next, IOptions<AppSettings> appsettings)
        {
            _next = next;
            _appsettings= appsettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            // Access the configuration value within the middleware
            GlobalApp.AppSettings = _appsettings;

            // Perform any processing using the configuration value

            await _next(context);
        }
    }
}
