using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amaris.ETL.DocBinariesToRnD.Models;
using Amaris.ETL.Mongo;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.SQL;
using Microsoft.Extensions.Hosting;

namespace Amaris.ETL.DocBinariesToRnD
{
    public class DocBinariesToRnDETL : IHostedService
    {
        private readonly RabbitMQPipeline<DocBinary, DocBinary> _pipeline;
        private readonly SQLExtractor<DocBinary> _extractor;
        private readonly MongoLoader<DocBinary> _loader;

        public DocBinariesToRnDETL(RabbitMQPipeline<DocBinary, DocBinary> pipeline, SQLExtractor<DocBinary> extractor, MongoLoader<DocBinary> loader)
        {
            _pipeline = pipeline;
            _extractor = extractor;
            _loader = loader;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                {
                    new List<Task>
                    {
                        _pipeline.Run(_loader, cancellationToken),
                        _pipeline.Run(_extractor, cancellationToken)
                    }.ForEach(t => t.Wait());
                } 
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => Console.WriteLine("The DocBinariesToRndETL was requested to Stop"));
        }
    }
}