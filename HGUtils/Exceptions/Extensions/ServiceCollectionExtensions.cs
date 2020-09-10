using HGUtils.Exceptions.Contracts;
using HGUtils.Exceptions.Middleware;
using HGUtils.Exceptions.Services;
using HGUtils.Helpers.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorHandler(this IServiceCollection services, IWebHostEnvironment env) =>
            services
                .AddScoped<IExceptionHandler, ExceptionHandlerService>()
                .UseIf(env.IsDevelopment(),
                    x => x.AddTransient<DevelopErrorMiddleware>(),
                    x => x.AddTransient<ErrorMiddleware>());
    }
}
