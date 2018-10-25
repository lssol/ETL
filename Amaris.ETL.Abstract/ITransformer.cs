using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Amaris.ETL.Abstract
{
    public interface IBulkTransformer<TInput, TOutput> 
        where TInput : class 
        where TOutput : class
    {
        IEnumerable<TOutput> Transform(IEnumerable<TInput> input);
        int PeriodBulk { get; }
        int MaxBulkSize { get; }
        int MaxBufferSize { get; }
    }
    public interface ITransformer<TInput, TOutput> 
        where TInput : class 
        where TOutput : class
    {
        TOutput Transform(TInput input);
    }
}