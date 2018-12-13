using MessagePack;

namespace Lykke.MarginTrading.OrderBookService.Contracts.Models
{
    [MessagePackObject]
    public class VolumePriceContract
    {
        [Key(0)]
        public decimal Volume { get; set; }

        [Key(1)]
        public decimal Price { get; set; }
    }
}