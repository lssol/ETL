using System;
using System.Collections.Generic;
using System.Threading;
using Amaris.ETL.Abstract;

namespace Amaris.ETL.SQL
{
    public class SQLExtractor<T> : IExtractor<T> where T : class
    {
        public IEnumerable<T> Extract(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}