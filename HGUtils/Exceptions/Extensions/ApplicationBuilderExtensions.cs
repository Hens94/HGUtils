using HGUtils.Exceptions.Middleware;
using HGUtils.Helpers.Common;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    public static partial class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder app, IHostEnvironment env) =>
            app.UseIf(env.IsDevelopment(),
                x => x.UseMiddleware<DevelopErrorMiddleware>(),
                x => x.UseMiddleware<ErrorMiddleware>());
    }
}
