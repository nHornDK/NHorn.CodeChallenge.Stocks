using Microsoft.Extensions.Hosting;
using NHorn.CodeChallenge.Stocks.Infrastructure.Repository;
using NHorn.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Services
{
    public class FakeStocksService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer timer;
        private TimeSpan executionDelay;
        private readonly StocksRepository stocksRepository;
        private Random random;

        public FakeStocksService(StocksRepository stocksRepository)
        {
            this.stocksRepository = stocksRepository;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("FakeStocksService is starting.");
            executionDelay = TimeSpan.FromSeconds(1);
            random = new Random();
            timer = new Timer(DoWork, null, TimeSpan.Zero, executionDelay);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            timer?.Change(Timeout.Infinite, 0);
            var count = Interlocked.Increment(ref executionCount);
            Console.WriteLine($"FakeStocksService. execution count: {count}");


            foreach (var s in stocksRepository.GetAll())
            {
                if (random.Next(0, 10) > 2)
                    continue;
                s.BidPrice = s.BidPrice * new decimal(random.NextDouble() + 0.8);
                s.AskPrice = s.AskPrice * new decimal(random.NextDouble() + 0.8);
                stocksRepository.Update(s);
            }

            timer.Change(executionDelay, TimeSpan.Zero);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("FakeStocksService is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
