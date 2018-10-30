using System;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.SQL.Test.Models;
using Amaris.Service;
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
                .DefaultConfiguration(args)
                .UseSQLExtractor<CandidateInput>()
                .UseRabbitMQPipeline<CandidateInput, CandidateOutput>()
                .AddHostedService<CandidateETL>()
                .Build();
            host.Run();
        }
    }
}