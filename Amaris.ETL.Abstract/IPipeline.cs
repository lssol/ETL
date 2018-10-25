using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Amaris.ETL.Abstract
{
    public interface IPipeline<TInput, TOutput> : IDisposable where TInput : class where TOutput : class
    {
        Task Run(IExtractor<TInput> extractor, CancellationToken token);
        Task Run(ITransformer<TInput, TOutput> transformer, CancellationToken token);
        Task Run(IBulkTransformer<TInput, TOutput> transformer, CancellationToken token);
        Task Run(ILoader<TOutput> loader, CancellationToken token);
    }
}