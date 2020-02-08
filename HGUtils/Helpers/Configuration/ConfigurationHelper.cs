using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HGUtils.Helpers.Configuration
{
    public static class ConfigurationHelper
    {
        public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfiguration config, string sectionName = null) where T : class
        {
            if (sectionName is null)
            {
                sectionName = typeof(T).Name;
            }

            var configuration = config.GetSection(sectionName).Get<T>();

            if (configuration is null)
            {
                return services;
            }

            return services.AddSingleton(configuration);
        }

        private static T Get<T>(this IConfiguration configuration)
            => configuration.Get<T>(_ => { });

        private static T Get<T>(this IConfiguration configuration, Action<BinderOptions> configureOptions)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var result = configuration.Get(typeof(T), configureOptions);
            if (result == null)
            {
                return default(T);
            }
            return (T)result;
        }
    }
}
