using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService
{
    [UsedImplicitly]
    public static class MappingExtensions
    {
        internal static List<ExternalOrderBookContract> ToContracts(this List<ExternalOrderBook> orderBooks)
        {
            return orderBooks.Select(x => x.ToContract()).ToList();
        }
        
        internal static ExternalOrderBookContract ToContract(this ExternalOrderBook orderBook)
        {
            return new ExternalOrderBookContract
            {
                ExchangeName = orderBook.ExchangeName,
                AssetPairId = orderBook.AssetPairId,
                Timestamp = orderBook.Timestamp,
                Asks = orderBook.Asks.ToContracts(),
                Bids = orderBook.Bids.ToContracts(),
            };
        }

        internal static List<VolumePriceContract> ToContracts(this List<VolumePrice> volumePrices)
        {
            return volumePrices.Select(x => new VolumePriceContract
            {
                Volume = x.Volume,
                Price = x.Price,
            }).ToList();
        }
    }
}