using System.Collections.Generic;
using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService.Core.Services
{
    public interface IOrderBooksProviderService
    {   
        Task<ExternalOrderBook> GetCurrentOrderBookAsync(string exchange, string assetPairId);
        
        Task<List<ExternalOrderBook>> GetCurrentOrderBooksAsync(string assetPairId = null);
    }
}