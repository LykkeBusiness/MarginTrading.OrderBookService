// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Services;

namespace MarginTrading.OrderBookService.ExecutionOrderBookBroker
{
    public static class MappingExtension
    {
        public static OrderExecutionOrderBook ToDomain(this OrderExecutionOrderBookContract contract)
        {
            var orderBook = new OrderExecutionOrderBook
            {
                OrderId = contract.OrderId,
                ExternalOrderId = contract.ExternalOrderId,
                OrderBook = new ExternalOrderBook
                {
                    ExchangeName = contract.OrderBook.ExchangeName,
                    AssetPairId = contract.OrderBook.AssetPairId,
                    Timestamp = contract.OrderBook.Timestamp,
                    Asks = contract.OrderBook.Asks.Select(x => new VolumePrice{ Volume = x.Volume, Price = x.Price}).ToList(),
                    Bids = contract.OrderBook.Bids.Select(x => new VolumePrice{ Volume = x.Volume, Price = x.Price}).ToList(),
                    ReceiveTimestamp = contract.OrderBook.ReceiveTimestamp
                },
                Volume = contract.Volume
            };

            orderBook.Spread = SpreadCalculator.CalculateSpread(orderBook, contract.Volume);

            return orderBook;
        }
    }
}
