using System;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService.Services
{
    public static class SpreadCalculator
    {
        

        public static decimal CalculateSpread(OrderExecutionOrderBook message, decimal volume)
        {
            var spread = 0M;
            var currentLevel = 0;
            var qtyLeft = Math.Abs(volume);

            while (qtyLeft > 0 && currentLevel < message.OrderBook.Asks.Count)
            {
                var currentLevelVolume = (volume > 0 
                    ? message.OrderBook.Asks[currentLevel].Volume
                    : message.OrderBook.Bids[currentLevel].Volume);
                
                spread += (qtyLeft - currentLevelVolume > 0 ? currentLevelVolume : qtyLeft)
                          * (message.OrderBook.Asks[currentLevel].Price - message.OrderBook.Bids[currentLevel].Price);

                qtyLeft -= currentLevelVolume;
                currentLevel++;
            }

            return spread;
        }
    }
}