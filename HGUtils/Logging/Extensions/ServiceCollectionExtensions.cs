using HGUtils.Logging.Contracts;
using HGUtils.Logging.Middleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogging<TImplementation>(
            this IServiceCollection services) where TImplementation : class, ILogging =>
            services
                .AddTransient<LoggingMiddleware>()
                .AddTransient<ILogging, TImplementation>();
    }
}