using System;
using Amaris.ETL.Abstract;

namespace Amaris.ETL.Toolbox.Loaders
{
    public class ConsoleLoader<T> : ILoader<T> where T : class
    {
        public void Load(T toImport)
        {
            Console.WriteLine(toImport);
        }
    }
}