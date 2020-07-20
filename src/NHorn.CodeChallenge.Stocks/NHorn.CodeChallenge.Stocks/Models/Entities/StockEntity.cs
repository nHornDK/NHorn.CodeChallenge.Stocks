using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Models.Entities
{
    public class StockEntity
    {
        public string Symbol { get; set; }
        public decimal BidPrice { get; set; }
        public decimal AskPrice { get; set; }
        public string PriceCurrency { get; set; }
        public Guid Id { get; set; }
    }
}
