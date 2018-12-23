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
    }
}