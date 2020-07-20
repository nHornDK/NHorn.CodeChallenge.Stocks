using NHorn.CodeChallenge.Stocks.Infrastructure.Events;
using NHorn.CodeChallenge.Stocks.Models.Entities;
using NHorn.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Infrastructure.Repository
{
    public class StocksRepository
    {
        private readonly IEventBus eventbus;
        private List<StockEntity> stocks;
        public StocksRepository(IEventBus eventbus)
        {
            stocks = new List<StockEntity>();
            stocks.Add(new StockEntity() { Id = Guid.NewGuid(), Symbol = "AAPL", AskPrice = new decimal(156.20), BidPrice = new decimal(156.99), PriceCurrency = "USD" });
            stocks.Add(new StockEntity() { Id = Guid.NewGuid(), Symbol = "IBM", AskPrice = new decimal(155.10), BidPrice = new decimal(160.85), PriceCurrency = "USD" });
            stocks.Add(new StockEntity() { Id = Guid.NewGuid(), Symbol = "MSFT", AskPrice = new decimal(78), BidPrice = new decimal(78.65), PriceCurrency = "USD" });
            this.eventbus = eventbus;
        }

        public IEnumerable<StockEntity> GetAll()
        {
            return stocks;
        }

        public bool Update(StockEntity updated)
        {
            var stock = stocks.FirstOrDefault(x => x.Id.Equals(updated.Id));
            if(stock != null)
            {
                stock = updated;
                Console.WriteLine("Stock updated");
                eventbus.Publish<StockChangedEvent>(new StockChangedEvent(stock));
                return true;
            }
            return false;

        }


    }
}
