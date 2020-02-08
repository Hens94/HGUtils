﻿using HGUtils.Helpers.Common;
using Serilog;
using Serilog.Events;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseFileLogging(
            this IWebHostBuilder webHostBuilder,
            Func<LoggerConfiguration, LoggerConfiguration> developConfig = null,
            Func<LoggerConfiguration, LoggerConfiguration> aditionalConfig = null) =>
            webHostBuilder
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .UseIf(hostingContext.HostingEnvironment.IsDevelopment(),
                        x => x
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .WriteTo.Console()
                            .AddLoggerConfiguration(developConfig),
                        x => x
                            .MinimumLevel.Warning()
                            .AddLoggerConfiguration(aditionalConfig)
                    )
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        path: $"{AppDomain.CurrentDomain.BaseDirectory}log\\{hostingContext.HostingEnvironment.ApplicationName}.txt",
                        rollingInterval: RollingInterval.Day));

        private static LoggerConfiguration AddLoggerConfiguration(
            this LoggerConfiguration configuration,
            Func<LoggerConfiguration, LoggerConfiguration> newConfiguration) =>
                newConfiguration is null ? configuration : newConfiguration(configuration);
    }
}
