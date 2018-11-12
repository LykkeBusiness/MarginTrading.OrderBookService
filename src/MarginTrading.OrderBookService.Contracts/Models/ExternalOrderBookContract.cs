using System;
using System.Collections.Generic;

namespace Lykke.MarginTrading.OrderBookService.Contracts.Models
{
    public class ExternalOrderBookContract
    {
        public string ExchangeName { get; set; }

        public string AssetPairId { get; set; }

        public DateTime Timestamp { get; set; }

        public List<VolumePriceContract> Asks { get; set; }

        public List<VolumePriceContract> Bids { get; set; }
    }
}