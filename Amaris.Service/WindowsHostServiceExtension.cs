using System;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Topshelf;

namespace Amaris.Service
{
    public static class WindowsHostServiceExtensions
    {
        /// <summary>
        ///     Runs a <c>IHost</c> as Service with Topshelf, and returns the exit code.
        ///         By default, only plugs the Start and Stop methods.
        ///         the <c>configureCallback</c> can be used to set the service name, display name, description, or credentials...
        /// </summary>
        /// <param name="host"></param>
        /// <param name="serviceSettings"></param>
        /// <returns>The exit code, from TopShelf <c>Run</c> method.</returns>
        public static int RunAsService(this IHost host, ServiceSettings serviceSettings = null)
        {
            var rc = HostFactory.Run(c =>
            {
                c.Service<IHost>(s =>
                {
                    var cts = new CancellationTokenSource();
                    s.ConstructUsing(f => host);
                    s.WhenStarted(async tc => await tc.StartAsync(cts.Token));
                    s.WhenStopped(async tc =>
                    {
                        cts.Cancel();
                        await tc.StopAsync();
                    });
                });
                c.SetServiceName(Assembly.GetEntryAssembly().GetName().Name);
                var settings = host.Services.GetService<ServiceSettings>() ?? serviceSettings;
                if (settings == null) return;
                c.SetServiceName(settings.ServiceName);
                c.SetDisplayName(settings.ServiceDisplayName ?? settings.ServiceName);
                c.SetDescription(settings.ServiceDescription ?? settings.ServiceDisplayName ?? settings.ServiceName);
            });
            return (int)Convert.ChangeType(rc, rc.GetTypeCode());
        }
    }
}