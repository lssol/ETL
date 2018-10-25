using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Amaris.ETL.Abstract;

namespace Amaris.ETL.SQL
{
    public class SQLExtractor<T> : IExtractor<T> where T : class
    {
        private readonly SQLExtractorOption _options;

        public SQLExtractor(SQLExtractorOption options)
        {
            _options = options;
        }

        public IEnumerable<T> Extract(CancellationToken token)
        {
            using (var repo = new Repository(_options.ConnectionString))
            {
                var offset = _options.InitialOffset;
                var limit = _options.Limit;
                var batchSize = _options.BatchSize;

                while (!token.IsCancellationRequested && (limit == -1 || offset < limit))
                {
                    var batch = repo.Query<T>(_options.StoredProcedureName, offset, batchSize);
                    offset = limit != -1 ? Math.Min(offset + batchSize, limit) : offset + batchSize;
                    if (batch.Any())
                        foreach (var elem in batch)
                            yield return elem;
                    else
                        yield break;
                    
                    Thread.Sleep(_options.QueryPeriod);
                }
            }
        }
    }
}