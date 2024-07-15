// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MarginTrading.OrderbookAggregator.Contracts.Messages;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService.OrderBookBroker
{
    public static class MappingExtension
    {
        public static ExternalOrderBook ToDomain(this ExternalExchangeOrderbookMessage orderBookMessage, DateTime now)
        {
            return new ExternalOrderBook
            {
                ExchangeName = orderBookMessage.ExchangeName,
                AssetPairId = orderBookMessage.AssetPairId,
                Timestamp = orderBookMessage.Timestamp,
                ReceiveTimestamp = now,
                Asks = orderBookMessage.Asks.ToDomain(),
                Bids = orderBookMessage.Bids.ToDomain(),
            };
        }

        private static List<Core.Domain.VolumePrice> ToDomain(
            this List<OrderbookAggregator.Contracts.Messages.VolumePrice> src)
        {
            return src.Select(x => new Core.Domain.VolumePrice { Volume = x.Volume, Price = x.Price, }).ToList();
        }
    }
}
