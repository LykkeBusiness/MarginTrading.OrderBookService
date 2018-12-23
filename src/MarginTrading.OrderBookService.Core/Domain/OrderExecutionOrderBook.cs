using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Domain
{
    public class OrderExecutionOrderBook: IOrderExecutionOrderBook
    {
        public string OrderId { get; set; }
        
        public decimal Volume { get; set; }
        
        public ExternalOrderBook OrderBook { get; set; }
    }
}