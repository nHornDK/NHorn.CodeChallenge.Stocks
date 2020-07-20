using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using NHorn.CodeChallenge.Stocks.Hubs;
using NHorn.CodeChallenge.Stocks.Infrastructure.Events;
using NHorn.CodeChallenge.Stocks.Models.Dto;
using NHorn.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Services
{
    public class NotifyFrontendService : IHostedService
    {
        private readonly IEventBus eventBus;
        private readonly IHubContext<StockHub, IStockHub> stockHubContext;
        private bool disposedValue;
        private IDisposable stockChangedDispose;

        public NotifyFrontendService(IEventBus eventBus, IHubContext<StockHub, IStockHub> stockHubContext)
        {
            this.eventBus = eventBus;
            this.stockHubContext = stockHubContext;
        }

        private void HandleStockChangedEventAsync(StockChangedEvent obj)
        {
            var dto = new StockDto()
            {
                Id = obj.Stock.Id.ToString(),
                Symbol = obj.Stock.Symbol,
                BidPrice = (double)obj.Stock.BidPrice,
                AskPrice = (double)obj.Stock.AskPrice
            };
            stockHubContext.Clients.All.StockChanged(dto);
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            stockChangedDispose = eventBus.Subscribe<StockChangedEvent>(HandleStockChangedEventAsync);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            eventBus.UnSubscribe<StockChangedEvent>(stockChangedDispose);
            return Task.CompletedTask;
        }
    }
}
