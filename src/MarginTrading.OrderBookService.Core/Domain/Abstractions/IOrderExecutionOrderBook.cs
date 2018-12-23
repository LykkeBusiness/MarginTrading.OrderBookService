namespace MarginTrading.OrderBookService.Core.Domain.Abstractions
{
    public interface IOrderExecutionOrderBook
    {
        string OrderId { get; }
        
        ExternalOrderBook OrderBook { get; }
    }
}