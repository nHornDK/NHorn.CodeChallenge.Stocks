using NHorn.CodeChallenge.Stocks.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Hubs
{
    public interface IStockHub
    {
        Task StockChanged(StockDto model);
        Task AllStocks(IEnumerable<StockDto> model);
    }
}
