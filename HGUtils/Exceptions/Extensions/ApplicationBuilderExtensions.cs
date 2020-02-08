using HGUtils.Exceptions.Middleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ErrorMiddleware>();
    }
}
