using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Amaris.Service
{
    public static class HostConfiguration
    {
        public static IHostBuilder DefaultConfiguration(this IHostBuilder hostBuilder, string[] args)
        {
            hostBuilder.ConfigureAppConfiguration((host, config) =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                config
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{env}.json", true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
            });
            hostBuilder.ConfigureServices(sp =>
            {
                sp.AddSingleton(s => s.GetService<IConfiguration>().GetSection("Amaris.Service").Get<ServiceSettings>());
            });
            return hostBuilder;
        }
        
        public static IHostBuilder AddHostedService<T>(this IHostBuilder hostBuilder) where T : class, IHostedService
        {
            hostBuilder.ConfigureServices(sp =>
            {
                sp.AddHostedService<T>();
            });
            return hostBuilder;
        }
    }
}