using System;
using System.Collections.Generic;
using MessagePack;

namespace Lykke.MarginTrading.OrderBookService.Contracts.Models
{
    [MessagePackObject]
    public class ExternalOrderBookContract
    {
        [Key(0)]
        public string ExchangeName { get; set; }

        [Key(1)]
        public string AssetPairId { get; set; }

        [Key(2)]
        public DateTime Timestamp { get; set; }

        [Key(3)]
        public List<VolumePriceContract> Asks { get; set; }

        [Key(4)]
        public List<VolumePriceContract> Bids { get; set; }
    }
}