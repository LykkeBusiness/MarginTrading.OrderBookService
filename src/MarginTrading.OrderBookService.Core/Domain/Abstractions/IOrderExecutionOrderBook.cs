// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace MarginTrading.OrderBookService.Core.Domain.Abstractions
{
    public interface IOrderExecutionOrderBook
    {
        string OrderId { get; }
        
        decimal Spread { get; }
        
        decimal Volume { get; }
        
        ExternalOrderBook OrderBook { get; }
        
        string ExternalOrderId { get; }
    }
}