using HGUtils.Exceptions.Middleware;
using HGUtils.Helpers.Common;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorMiddleware(this IServiceCollection services, IHostEnvironment env) =>
            services
                .UseIf(env.IsDevelopment(),
                    x => x.AddTransient<DevelopErrorMiddleware>(),
                    x => x.AddTransient<ErrorMiddleware>());
    }
}
