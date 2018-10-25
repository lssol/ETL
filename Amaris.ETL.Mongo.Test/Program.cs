using System;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.Toolbox.TestTools.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Amaris.ETL.Mongo.Test
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
                        .AddHostedService<CatToDogToMongoETL>()
                        .Configure<MongoOptions>(context.Configuration.GetSection("ETL.Loader.Mongo"))
                        .Configure<RabbitMQSettings>(context.Configuration.GetSection("ETL.RabbitMQ"))
                        .AddSingleton((s) => new RabbitMQPipeline<Cat, Dog>(s.GetService<IOptions<RabbitMQSettings>>().Value))
                        .AddSingleton((s) => new MongoLoader<Dog>(s.GetService<IOptions<MongoOptions>>().Value))
                        ;
                })
                .Build();
            host.Run();
        }
    }
}