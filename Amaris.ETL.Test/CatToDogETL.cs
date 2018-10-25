using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.Test.ETL;
using Amaris.ETL.Toolbox.Loaders;
using Amaris.ETL.Toolbox.TestTools.Extractors;
using Amaris.ETL.Toolbox.TestTools.Models;

namespace Amaris.ETL.Test
{
    public static class CatToDogETL
    {
        public static void RunETL(CancellationToken token, bool bulk = false)
        {
            using (var pipeline = new RabbitMQPipeline<Cat, Dog>(new RabbitMQSettings {ConnectionString = "host=localhost"}))
            {
                var tasks = new List<Task>
                {
                    pipeline.Run(new DummyDataExtractor(), token),
                    bulk ? pipeline.Run(new BulkTransformer(), token) : pipeline.Run(new Transformer(), token),
                    pipeline.Run(new ConsoleLoader<Dog>(), token)
                };

                tasks.ForEach(t => t.Wait());
            }
        }
    }
}