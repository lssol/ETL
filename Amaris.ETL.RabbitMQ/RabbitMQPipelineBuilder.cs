using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Amaris.ETL.RabbitMQ
{
    public static class RabbitMQPipelineBuilder
    {
        public static IHostBuilder UseRabbitMQPipeline<TInput, TOutput>(this IHostBuilder host) where TOutput : class where TInput : class
        {
            return host.ConfigureServices((context, sp) =>
            {
                sp
                    .Configure<RabbitMQSettings>(context.Configuration.GetSection("ETL.Pipeline.RabbitMQ"))
                    .AddSingleton((s) =>
                        new RabbitMQPipeline<TInput, TOutput>(s.GetService<IOptions<RabbitMQSettings>>().Value));
            });
        }
    }
}