using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Domain
{
    public class OrderExecutionOrderBook: IOrderExecutionOrderBook
    {
        public string OrderId { get; set; }
        
        public ExternalOrderBook OrderBook { get; set; }
        
        public decimal Spread { get; set; }
    }
}