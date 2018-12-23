using System.Collections.Generic;
using System.Linq;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService.ExecutionOrderBookBroker
{
    public static class MappingExtension
    {
        public static OrderExecutionOrderBook ToDomain(this OrderExecutionOrderBookContract contract)
        {
            return new OrderExecutionOrderBook
            {
                OrderId = contract.OrderId,
                Volume = contract.Volume,
                OrderBook = new ExternalOrderBook
                {
                    ExchangeName = contract.OrderBook.ExchangeName,
                    AssetPairId = contract.OrderBook.AssetPairId,
                    Timestamp = contract.OrderBook.Timestamp,
                    Asks = contract.OrderBook.Asks.Select(x => new VolumePrice{ Volume = x.Volume, Price = x.Price}).ToList(),
                    Bids = contract.OrderBook.Bids.Select(x => new VolumePrice{ Volume = x.Volume, Price = x.Price}).ToList(),
                },
            };
        }
    }
}
