using HGUtils.Logging.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<LoggingMiddleware>();
    }
}