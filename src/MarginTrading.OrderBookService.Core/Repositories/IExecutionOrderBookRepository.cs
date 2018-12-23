using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Repositories
{
    public interface IExecutionOrderBookRepository
    {
        Task AddAsync(IOrderExecutionOrderBook orderBook);
        
        Task<IOrderExecutionOrderBook> GetAsync(string orderId);
    }
}