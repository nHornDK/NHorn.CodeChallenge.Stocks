using NHorn.CodeChallenge.Stocks.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Infrastructure.Events
{
    public class StockChangedEvent
    {
        public StockChangedEvent(StockEntity stock)
        {
            Stock = stock;
        }

        public StockEntity Stock { get; }
    }
}
