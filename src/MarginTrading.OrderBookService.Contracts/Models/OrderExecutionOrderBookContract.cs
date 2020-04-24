// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using MessagePack;

namespace Lykke.MarginTrading.OrderBookService.Contracts.Models
{
    /// <summary>
    /// LOP order execution orderbook 
    /// </summary>
    [MessagePackObject]
    public class OrderExecutionOrderBookContract
    {
        /// <summary>
        /// Order ID
        /// </summary>
        [Key(0)]
        public string OrderId { get; set; }
        
        /// <summary>
        /// Order execution signed volume 
        /// </summary>
        [Key(1)]
        public decimal Volume { get; set; }
        
        /// <summary>
        /// Orderbook
        /// </summary>
        [Key(2)]
        public ExternalOrderBookContract OrderBook { get; set; }
        
        /// <summary>
        /// External order id
        /// </summary>
        [Key(3)]
        public string ExternalOrderId { get; set; }
    }
}