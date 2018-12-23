namespace MarginTrading.OrderBookService.Core.Domain.Abstractions
{
    public interface IOrderExecutionOrderBook
    {
        string OrderId { get; }
        
        decimal Spread { get; }
        
        ExternalOrderBook OrderBook { get; }
    }
}