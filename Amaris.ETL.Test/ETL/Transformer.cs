using Amaris.ETL.Abstract;
using Amaris.ETL.Toolbox.TestTools.Models;

namespace Amaris.ETL.Test.ETL
{
    public class Transformer : ITransformer<Cat, Dog>
    {
        public Dog Transform(Cat input)
        {
            return new Dog
            {
                Name = $"{input.Name} the dog",
                Race = input.Color
            };
        }
    }
}