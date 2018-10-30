using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Amaris.ETL.SQL
{
    public static class SQLExtractorBuilder
    {
        public static IHostBuilder UseSQLExtractor<T>(this IHostBuilder host) where T : class
        {
            return host.ConfigureServices((context, sp) =>
            {
                sp
                    .Configure<SQLExtractorOption>(context.Configuration.GetSection("ETL.Extractor.SQL"))
                    .AddSingleton((s) => new SQLExtractor<T>(s.GetService<IOptions<SQLExtractorOption>>().Value));
            });
        }
    }
}