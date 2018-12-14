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
        
        ExternalOrderBook IOrderExecutionOrderBook.OrderBook => new ExternalOrderBook
        {
            ExchangeName = ExchangeName,
            AssetPairId = AssetPairId,
            Timestamp = Timestamp,
            Asks = JsonConvert.DeserializeObject<List<VolumePrice>>(Asks),
            Bids = JsonConvert.DeserializeObject<List<VolumePrice>>(Bids),
        };
        
        public string ExchangeName { get; set; }

        public string AssetPairId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Asks { get; set; }

        public string Bids { get; set; }

        public static OrderExecutionOrderBookEntity Create(IOrderExecutionOrderBook orderBook)
        {
            return new OrderExecutionOrderBookEntity
            {
                OrderId = orderBook.OrderId,
                ExchangeName = orderBook.OrderBook.ExchangeName,
                AssetPairId = orderBook.OrderBook.AssetPairId,
                Timestamp = orderBook.OrderBook.Timestamp,
                Asks = orderBook.OrderBook.ToJson(),
                Bids = orderBook.OrderBook.ToJson(),
            };
        }
    }
}