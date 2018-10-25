using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.SQL.Test.Models;
using Amaris.ETL.Toolbox.Loaders;
using Amaris.ETL.Toolbox.Transformers;
using Microsoft.Extensions.Hosting;

namespace Amaris.ETL.SQL.Test
{
    public class CandidateETL : IHostedService
    {
        private RabbitMQPipeline<CandidateInput, CandidateOutput> _pipeline;
        private SQLExtractor<CandidateInput> _extractor;

        public CandidateETL(RabbitMQPipeline<CandidateInput, CandidateOutput> pipeline, SQLExtractor<CandidateInput> extractor)
        {
            _pipeline = pipeline;
            _extractor = extractor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var transformer = new SimpleTransformer<CandidateInput, CandidateOutput>(c => new CandidateOutput
                {
                    Email = c.Email,
                    Firstname = c.Firstname,
                    Lastname = c.Lastname
                });
                var tasks = new List<Task>
                {
                    _pipeline.Run(new ConsoleLoader<CandidateOutput>(), cancellationToken),
                    _pipeline.Run(transformer, cancellationToken),
                    _pipeline.Run(_extractor, cancellationToken),
                };
                tasks.ForEach(t => t.Wait());
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => Console.WriteLine("The Candidate ETL was requested to Stop"));
        }
    }
}