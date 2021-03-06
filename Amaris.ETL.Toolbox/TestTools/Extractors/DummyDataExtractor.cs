using System;
using System.Collections.Generic;
using System.Threading;
using Amaris.ETL.Abstract;
using Amaris.ETL.Toolbox.TestTools.Models;

namespace Amaris.ETL.Toolbox.TestTools.Extractors
{
    public class DummyDataExtractor : IExtractor<Cat>
    {
        public IEnumerable<Cat> Extract(CancellationToken token)
        {
            var names = new[] {"scott", "felix", "brian", "shawn"};
            var colors = new[] {"red", "yellow", "green", "brown"};
            var rnd = new Random();
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(100);

                yield return new Cat
                {
                    Name = names[rnd.Next(names.Length)],
                    Color =  colors[rnd.Next(colors.Length)]
                };
            }
        }
    }
}