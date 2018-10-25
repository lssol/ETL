using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.Toolbox.TestTools.Extractors;
using Amaris.ETL.Toolbox.TestTools.Models;
using Amaris.ETL.Toolbox.Transformers;
using Microsoft.Extensions.Hosting;

namespace Amaris.ETL.Mongo.Test
{
    public class CatToDogToMongoETL : IHostedService
    {
        private RabbitMQPipeline<Cat, Dog> _pipeline;
        private MongoLoader<Dog> _loader;

        public CatToDogToMongoETL(RabbitMQPipeline<Cat, Dog> pipeline, MongoLoader<Dog> loader)
        {
            _pipeline = pipeline;
            _loader = loader;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var transformer = new SimpleTransformer<Cat, Dog>((cat => new Dog
                {
                    Name = $"{cat.Name} the dog",
                    Race = cat.Color
                }));
                var tasks = new List<Task>
                {
                    _pipeline.Run(_loader, cancellationToken),
                    _pipeline.Run(transformer, cancellationToken),
                    _pipeline.Run(new DummyDataExtractor(), cancellationToken),
                };
                tasks.ForEach(t => t.Wait());
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => Console.WriteLine("The Mongo ETL was requested to Stop"));
        }
    }
}