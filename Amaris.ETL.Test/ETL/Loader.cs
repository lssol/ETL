using System;
using Amaris.ETL.Abstract;
using Amaris.ETL.Test.Models;

namespace Amaris.ETL.Test.ETL
{
    public class Loader : ILoader<Dog>
    {
        public void Load(Dog toImport)
        {
            Console.WriteLine($"The following dog was imported: {toImport}");
        }
    }
}