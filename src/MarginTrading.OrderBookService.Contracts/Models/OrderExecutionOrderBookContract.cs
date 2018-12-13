using MessagePack;

namespace Lykke.MarginTrading.OrderBookService.Contracts.Models
{
    [MessagePackObject]
    public class OrderExecutionOrderBookContract
    {
        [Key(0)]
        public string OrderId { get; set; }
        
        [Key(1)]
        public ExternalOrderBookContract OrderBook { get; set; }
    }
}