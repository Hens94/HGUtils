using HGUtils.Logging.Middleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<LoggingMiddleware>();
    }
}