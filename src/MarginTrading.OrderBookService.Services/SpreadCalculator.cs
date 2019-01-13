using System;
using System.Collections.Generic;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService.Services
{
    public static class SpreadCalculator
    {
        

        public static decimal CalculateSpread(OrderExecutionOrderBook message, decimal volume)
        {
            decimal SpreadWeight(IReadOnlyList<VolumePrice> orderbook)
            {
                var weight = 0M;
                var currentLevel = 0;
                var qtyLeft = Math.Abs(volume);

                while (qtyLeft > 0 && currentLevel < message.OrderBook.Asks.Count)
                {
                    var currentLevelVolume = currentLevel != orderbook.Count - 1 
                        ? orderbook[currentLevel].Volume
                        : decimal.MaxValue;

                    weight += (qtyLeft - currentLevelVolume > 0 ? currentLevelVolume : qtyLeft)
                              * orderbook[currentLevel].Price;
                    qtyLeft -= currentLevelVolume;
                    currentLevel++;
                }

                return weight;
            }

            var askSpreadWeight = SpreadWeight(message.OrderBook.Asks);
            var bidSpreadWeight = SpreadWeight(message.OrderBook.Bids);

            return askSpreadWeight - bidSpreadWeight;
        }
        
        
    }
}