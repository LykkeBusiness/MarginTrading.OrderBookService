using System;
using System.Collections.Generic;
using System.Linq;
using MarginTrading.Backend.Contracts.Events;
using MarginTrading.Backend.Contracts.Orders;
using MarginTrading.OrderbookAggregator.Contracts.Messages;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService.OrderBookBroker
{
    public static class MappingExtension
    {
        public static ExternalOrderBook ToDomain(this ExternalExchangeOrderbookMessage orderBookMessage)
        {
            return new ExternalOrderBook
            {
                ExchangeName = orderBookMessage.ExchangeName,
                AssetPairId = orderBookMessage.AssetPairId,
                Timestamp = orderBookMessage.Timestamp,
                Asks = orderBookMessage.Asks.ToDomain(),
                Bids = orderBookMessage.Bids.ToDomain(),
            };
        }

        private static List<Core.Domain.VolumePrice> ToDomain(this List<OrderbookAggregator.Contracts.Messages.VolumePrice> src)
        {
            return src.Select(x => new Core.Domain.VolumePrice
            {
                Volume = x.Volume,
                Price = x.Price,
            }).ToList();
        }
    }
}
