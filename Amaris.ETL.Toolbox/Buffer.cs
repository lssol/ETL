using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Amaris.ETL.Toolbox
{
    public class Buffer<T>
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly int _period;
        private readonly int _batchSize;
        
        public Task BufferProcessor { get; private set; }
        
        public Buffer(int period = 10, int batchSize = 10)
        {
            _period = period;
            _batchSize = batchSize;
        }
        
        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }

        public Task Run(Action<IEnumerable<T>> sendBatch)
        {
            if (!BufferProcessor?.IsCompleted ?? false)
                return BufferProcessor;
            return BufferProcessor = Task.Run(() =>
            {
                while (true)
                {
                    var toPublish = new List<T>();
                    for (var i = 0; i < _batchSize; ++i)
                    {
                        if (!_queue.TryDequeue(out var item))
                            break;
                        toPublish.Add(item);
                    }

                    if (!toPublish.Any())
                        break;

                    try
                    {
                        sendBatch(toPublish);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetCurrentClassLogger().Error(e, "The buffer crashed when attempting to send the batch");
                        throw;
                    }

                    Thread.Sleep(_period);
                }
            });
        }
    }
}