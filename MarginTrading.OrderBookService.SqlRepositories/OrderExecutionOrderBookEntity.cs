// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Common;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;
using Newtonsoft.Json;

namespace MarginTrading.OrderBookService.SqlRepositories
{
    public class OrderExecutionOrderBookEntity: IOrderExecutionOrderBook
    {
        public string OrderId { get; set; }
        
        public decimal Spread { get; set; }
        
        public decimal Volume { get; set; }
        
        ExternalOrderBook IOrderExecutionOrderBook.OrderBook => new ExternalOrderBook
        {
            ExchangeName = ExchangeName,
            AssetPairId = AssetPairId,
            Timestamp = Timestamp,
            Asks = JsonConvert.DeserializeObject<List<VolumePrice>>(Asks),
            Bids = JsonConvert.DeserializeObject<List<VolumePrice>>(Bids),
            ReceiveTimestamp = ReceiveTimestamp
        };

        public string ExternalOrderId { get; set; }

        public string ExchangeName { get; set; }

        public string AssetPairId { get; set; }

        public DateTime Timestamp { get; set; }
        
        public DateTime ReceiveTimestamp { get; set; }

        public string Asks { get; set; }

        public string Bids { get; set; }

        public static OrderExecutionOrderBookEntity Create(IOrderExecutionOrderBook orderBook)
        {
            return new OrderExecutionOrderBookEntity
            {
                OrderId = orderBook.OrderId,
                ExternalOrderId = orderBook.ExternalOrderId,
                Spread = orderBook.Spread,
                ExchangeName = orderBook.OrderBook.ExchangeName,
                AssetPairId = orderBook.OrderBook.AssetPairId,
                Timestamp = orderBook.OrderBook.Timestamp,
                Asks = orderBook.OrderBook.Asks.ToJson(),
                Bids = orderBook.OrderBook.Bids.ToJson(),
                Volume = orderBook.Volume,
                ReceiveTimestamp = orderBook.OrderBook.ReceiveTimestamp
            };
        }
    }
}