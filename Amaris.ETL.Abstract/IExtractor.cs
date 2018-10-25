using System;
using System.Collections.Generic;
using System.Threading;

namespace Amaris.ETL.Abstract
{
    public interface IExtractor<TInput> where TInput : class
    {
        IEnumerable<TInput> Extract(CancellationToken token);
    }
}