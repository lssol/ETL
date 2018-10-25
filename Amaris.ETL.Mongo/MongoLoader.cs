using System;
using System.Collections.Generic;
using System.Linq;
using Amaris.ETL.Abstract;
using Amaris.ETL.Toolbox;
using MongoDB.Driver;

namespace Amaris.ETL.Mongo
{
    public class MongoLoader<T> : ILoader<T> where T : class
    {
        private readonly MongoOptions  _options;
        private readonly IMongoCollection<T> _collection;
        private readonly Buffer<T> _buffer;

        public MongoLoader(MongoOptions options)
        {
            _options = options;
            _collection = new MongoClient()
                .GetDatabase(_options.Database)
                .GetCollection<T>(_options.Collection);
            _buffer = new Buffer<T>(_options.Period, _options.BatchSize);
        }

        public void Load(T toImport)
        {
            _buffer.Run(t => _collection.InsertMany(t));
            if (toImport != null)
                _buffer.Enqueue(toImport);
        }
    }
}