// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Domain
{
    public class OrderExecutionOrderBook: IOrderExecutionOrderBook
    {
        public string OrderId { get; set; }
        
        public decimal Spread { get; set; }
        
        public ExternalOrderBook OrderBook { get; set; }
    }
}