﻿using HGUtils.Exceptions.Middleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorMiddleware(this IServiceCollection services) =>
            services
                .AddTransient<ErrorMiddleware>();
    }
}
