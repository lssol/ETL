using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Amaris.ETL.Mongo
{
    public static class MongoLoaderBuilder
    {
        public static IHostBuilder UseMongoLoader<T>(this IHostBuilder host) where T : class
        {
            return host.ConfigureServices((context, sp) =>
            {
                sp
                    .Configure<MongoLoaderOptions>(context.Configuration.GetSection("ETL.Loader.Mongo"))
                    .AddSingleton((s) => new MongoLoader<T>(s.GetService<IOptions<MongoLoaderOptions>>().Value));
            });
        }
    }
}