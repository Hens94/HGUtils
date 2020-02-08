using HGUtils.Exceptions.Middleware;
using HGUtils.Helpers.Common;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder app, IHostEnvironment env) =>
            app.UseIf(env.IsDevelopment(),
                x => x.UseMiddleware<DevelopErrorMiddleware>(),
                x => x.UseMiddleware<ErrorMiddleware>());
    }
}
