using System;

namespace Amaris.ETL.Abstract
{
    public interface ILoader<TOutput> where TOutput : class
    {
        void Load(TOutput toImport);
    }
}