using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Models.Dto
{
    public class StockDto
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public double BidPrice { get; set; }
        public double AskPrice { get; set; }
    }
}
