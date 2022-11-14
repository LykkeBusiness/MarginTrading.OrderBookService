// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;

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
                ReceiveTimestamp = orderBook.ReceiveTimestamp,
                Asks = orderBook.Asks.ToContracts(),
                Bids = orderBook.Bids.ToContracts(),
            };
        }
        
        internal static OrderExecutionOrderBookContract ToContract(this IOrderExecutionOrderBook orderBook)
        {
            return new OrderExecutionOrderBookContract
            {
                OrderId = orderBook.OrderId,
                ExternalOrderId = orderBook.ExternalOrderId,
                OrderBook = orderBook.OrderBook.ToContract(),
                Volume = orderBook.Volume
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