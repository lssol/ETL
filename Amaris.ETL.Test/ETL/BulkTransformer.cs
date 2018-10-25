using System.Collections.Generic;
using System.Linq;
using Amaris.ETL.Abstract;
using Amaris.ETL.Test.Models;

namespace Amaris.ETL.Test.ETL
{
    public class BulkTransformer : IBulkTransformer<Cat, Dog>
    {
        public IEnumerable<Dog> Transform(IEnumerable<Cat> input)
        {
            return input.Select(i => new Dog
            {
                Race = i.Color,
                Name = i.Name
            });
        }

        public int PeriodBulk => 100;
        public int MaxBulkSize => 200;
        public int MaxBufferSize => 50;
    }
}