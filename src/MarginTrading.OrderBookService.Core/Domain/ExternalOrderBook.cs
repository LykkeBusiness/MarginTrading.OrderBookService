// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Domain
{
    public class ExternalOrderBook
    {
        public string ExchangeName { get; set; }

        public string AssetPairId { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime ReceiveTimestamp { get; set; }

        public List<VolumePrice> Asks { get; set; }

        public List<VolumePrice> Bids { get; set; }
        
        
    }
}