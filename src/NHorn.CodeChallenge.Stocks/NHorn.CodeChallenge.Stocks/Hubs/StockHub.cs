using NHorn.CodeChallenge.Stocks.Infrastructure.Events;
using NHorn.CodeChallenge.Stocks.Infrastructure.Repository;
using NHorn.CodeChallenge.Stocks.Models.Dto;
using NHorn.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Hubs
{
    public class StockHub : HubBase<IStockHub>
    {
        private readonly IEventBus eventBus;
        private readonly StocksRepository stocksRepository;

        public StockHub(IEventBus eventBus, StocksRepository stocksRepository) : base(eventBus)
        {
            this.eventBus = eventBus;
            this.stocksRepository = stocksRepository;
        }

        public DateTime PingPong(DateTime model)
        {
            return model;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected");
            var allStocks = stocksRepository.GetAll().Select(obj => new StockDto()
            {
                Id = obj.Id.ToString(),
                Symbol = obj.Symbol,
                BidPrice = (double)obj.BidPrice,
                AskPrice = (double)obj.AskPrice
            });
            await Clients.Caller.AllStocks(allStocks);
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Console.WriteLine("Client disconnected");
            await Task.CompletedTask;
        }
    }
}
