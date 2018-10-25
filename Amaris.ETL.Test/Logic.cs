using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amaris.ETL.RabbitMQ;
using Amaris.ETL.Test.ETL;
using Amaris.ETL.Test.Models;

namespace Amaris.ETL.Test
{
    public static class Logic
    {
        public static void RunETL(CancellationToken token, bool bulk = false)
        {
            using (var pipeline = new RabbitMQPipeline<Cat, Dog>(new RabbitMQSettings {ConnectionString = "host=localhost"}))
            {
                var tasks = new List<Task>
                {
                    pipeline.Run(new Extractor(), token),
                    bulk ? pipeline.Run(new BulkTransformer(), token) : pipeline.Run(new Transformer(), token),
                    pipeline.Run(new Loader(), token)
                };

                tasks.ForEach(t => t.Wait());
            }
        }
    }
}