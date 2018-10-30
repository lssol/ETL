using Amaris.ETL.RabbitMQ;
using Amaris.ETL.Toolbox.TestTools.Models;
using Amaris.Service;
using Microsoft.Extensions.Hosting;

namespace Amaris.ETL.Mongo.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .DefaultConfiguration(args)
                .UseMongoLoader<Dog>()
                .UseRabbitMQPipeline<Cat, Dog>()
                .AddHostedService<CatToDogToMongoETL>()
                .Build();
            host.Run();
        }
    }
}