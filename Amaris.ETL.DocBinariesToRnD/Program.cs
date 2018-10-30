using System;
using Amaris.ETL.DocBinariesToRnD.Models;
using Amaris.ETL.Mongo;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.SQL;
using Microsoft.Extensions.Configuration;
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
                .ConfigureAppConfiguration((_, config) =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    config
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{env}.json", true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureServices((context, sp) =>
                {
                    sp
                        .AddHostedService<DocBinariesToRnDETL>()
                        .Configure<SQLExtractorOption>(context.Configuration.GetSection("ETL.Extractor.SQL"))
                        .Configure<MongoLoaderOptions>(context.Configuration.GetSection("ETL.Loader.Mongo"))
                        .Configure<RabbitMQSettings>(context.Configuration.GetSection("ETL.RabbitMQ"))
                        .AddSingleton((s) => new RabbitMQPipeline<DocBinary, DocBinary>(s.GetService<IOptions<RabbitMQSettings>>().Value))
                        .AddSingleton((s) => new SQLExtractor<DocBinary>(s.GetService<IOptions<SQLExtractorOption>>().Value))
                        .AddSingleton((s) => new MongoLoader<DocBinary>(s.GetService<IOptions<MongoLoaderOptions>>().Value))
                        ;
                })
                .Build();
            host.Run();
        }
    }
}