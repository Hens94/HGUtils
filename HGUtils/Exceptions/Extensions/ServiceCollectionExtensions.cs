using HGUtils.Exceptions.Middleware;
using HGUtils.Helpers.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorMiddleware(this IServiceCollection services, IWebHostEnvironment env) =>
            services
                .UseIf(env.IsDevelopment(),
                    x => x.AddTransient<DevelopErrorMiddleware>(),
                    x => x.AddTransient<ErrorMiddleware>());
    }
}
