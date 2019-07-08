// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

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