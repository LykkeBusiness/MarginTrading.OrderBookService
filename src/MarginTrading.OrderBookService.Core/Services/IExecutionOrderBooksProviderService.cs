using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Services
{
    public interface IExecutionOrderBooksProviderService
    {
        Task<IOrderExecutionOrderBook> GetAsync(string orderId);
    }
}