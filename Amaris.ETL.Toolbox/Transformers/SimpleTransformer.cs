using System;
using Amaris.ETL.Abstract;

namespace Amaris.ETL.Toolbox.Transformers
{
    public class SimpleTransformer<TInput, TOutput> : ITransformer<TInput, TOutput> where TOutput : class where TInput : class
    {
        private Func<TInput, TOutput> transform;

        public SimpleTransformer(Func<TInput, TOutput> transform)
        {
            this.transform = transform;
        }

        public TOutput Transform(TInput input)
        {
            return transform(input);
        }
    }
}