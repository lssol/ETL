﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Amaris.ETL.Abstract;
using EasyNetQ;
using EasyNetQ.NonGeneric;
using NLog;

namespace Amaris.ETL.RabbitMQ
{
    public class RabbitMQPipeline<TInput, TOutput> : IPipeline<TInput, TOutput> where TInput : class where TOutput : class
    {
        private readonly IBus _bus;

        public RabbitMQPipeline(RabbitMQSettings settings)
        {
            _bus = RabbitHutch.CreateBus(settings.ConnectionString);
        }

        public Task Run(IExtractor<TInput> extractor, CancellationToken token)
        {
            return Task.Run(() =>
            {
                foreach (var extracted in extractor.Extract(token))
                    _bus.Publish(extracted);
            });
        }

        public Task Run(ITransformer<TInput, TOutput> transformer, CancellationToken token)
        {
            ThrowIfTInputEqualsTOutput();
            return Task.Run(() =>
            {
                var subscriptionId = GetSubscriptionId();
                _bus.Subscribe(subscriptionId, (TInput input) =>
                {
                    var result = transformer.Transform(input);
                    _bus.Publish(result);
                });
            });
        }

        public Task Run(IBulkTransformer<TInput, TOutput> transformer, CancellationToken token)
        {
            ThrowIfTInputEqualsTOutput();
            return Task.Run(() =>
            {
                var subscriptionId = GetSubscriptionId();
                var buffer = new ConcurrentQueue<TInput>();
                _bus.Subscribe(subscriptionId, (TInput input) =>
                {
                    while (buffer.Count >= transformer.MaxBufferSize)
                    {
                        if (token.IsCancellationRequested)
                            return;
                        LogManager.GetCurrentClassLogger().Debug($"Max Size Buffer reached: {buffer.Count}");
                        Thread.Sleep(100);
                    }
                    buffer.Enqueue(input);
                });
                while (!token.IsCancellationRequested)
                {
                    var bulk = new List<TInput>();
                    while (bulk.Count <= transformer.MaxBufferSize && buffer.TryDequeue(out var input))
                        bulk.Add(input);
                    transformer.Transform(bulk).ToList().ForEach(_bus.Publish);
                    
                    Thread.Sleep(transformer.PeriodBulk);
                }
            });
        }

        public Task Run(ILoader<TOutput> loader, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var subscriptionId = GetSubscriptionId();
                _bus.Subscribe(subscriptionId, (TOutput output) => { loader.Load(output); });
            });
        }

        private static void ThrowIfTInputEqualsTOutput()
        {
            if (typeof(TInput) == typeof(TOutput))
                throw new Exception("If TInput = TOutput, you cannot use a transformer. Make two classes.");
        }

        private static string GetSubscriptionId()
        {
            return "";
        }
        
        public void Dispose()
        {
            _bus?.Dispose();
        }
    }
}