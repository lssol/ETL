using Amaris.ETL.DocBinariesToRnD.Models;
using Amaris.ETL.Mongo;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.SQL;
using Amaris.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Amaris.ETL.DocBinariesToRnD
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .DefaultConfiguration(args)
                .UseRabbitMQPipeline<DocBinary, DocBinary>()
                .UseSQLExtractor<DocBinary>()
                .UseMongoLoader<DocBinary>()
                .AddHostedService<DocBinariesToRnDETL>()
                .Build();
            host.RunAsService();
        }
    }
}