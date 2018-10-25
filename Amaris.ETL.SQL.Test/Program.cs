using System;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.SQL.Test.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Amaris.ETL.SQL.Test
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
                        .AddHostedService<CandidateETL>()
                        .Configure<SQLExtractorOption>(context.Configuration.GetSection("ETL.SQL"))
                        .Configure<RabbitMQSettings>(context.Configuration.GetSection("ETL.RabbitMQ"))
                        .AddSingleton((s) => new RabbitMQPipeline<CandidateInput, CandidateOutput>(s.GetService<IOptions<RabbitMQSettings>>().Value))
                        .AddSingleton((s) => new SQLExtractor<CandidateInput>(s.GetService<IOptions<SQLExtractorOption>>().Value))
                    ;
                })
                .Build();
            host.Run();
        }
    }
}