using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HGUtils.Helpers.Configuration
{
    public static class ConfigurationHelper
    {
        public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfiguration config, string sectionName = null) where T : class
        {
            var configuration = config.GetConfig<T>(sectionName);

            if (configuration is null)
            {
                return services;
            }

            return services.AddSingleton(configuration);
        }

        public static T GetConfig<T>(this IConfiguration config, string sectionName = null) where T : class
        {
            if (sectionName is null)
            {
                sectionName = typeof(T).Name;
            }

            return config.GetSection(sectionName).Get<T>();
        }
    }
}
