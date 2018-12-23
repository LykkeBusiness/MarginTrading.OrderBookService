using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MarginTrading.OrderBookService.Core.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MarginTrading.OrderBookService.Tests
{
    public class SpreadCalculatorDataSource
    {
        public static IEnumerable Cases()
        {
            yield return new TestCaseData(new OrderExecutionOrderBook
            {
                OrderBook = new ExternalOrderBook
                {
                    Asks = new []{ new VolumePrice { Volume = 100, Price = 10}, new VolumePrice { Volume = 100, Price = 11} }.ToList(),
                    Bids = new []{ new VolumePrice { Volume = 50, Price = 9}, new VolumePrice { Volume = 50, Price = 8.5M} }.ToList(),
                }
            }, 110M).Returns(125M);
            
            yield return new TestCaseData(new OrderExecutionOrderBook
            {
                OrderBook = new ExternalOrderBook
                {
                    Asks = new []{ new VolumePrice { Volume = 100, Price = 10}, new VolumePrice { Volume = 100, Price = 11} }.ToList(),
                    Bids = new []{ new VolumePrice { Volume = 50, Price = 9}, new VolumePrice { Volume = 50, Price = 8.5M} }.ToList(),
                }
            }, 99M).Returns(99M);
            
            yield return new TestCaseData(new OrderExecutionOrderBook
            {
                OrderBook = new ExternalOrderBook
                {
                    Asks = new []{ new VolumePrice { Volume = 100, Price = 10}, new VolumePrice { Volume = 100, Price = 11} }.ToList(),
                    Bids = new []{ new VolumePrice { Volume = 50, Price = 9}, new VolumePrice { Volume = 60, Price = 8.5M} }.ToList(),
                }
            }, -110M).Returns(200M);
            
            yield return new TestCaseData(new OrderExecutionOrderBook
            {
                OrderBook = new ExternalOrderBook
                {
                    Asks = new []{ new VolumePrice { Volume = 100, Price = 10}, new VolumePrice { Volume = 100, Price = 11} }.ToList(),
                    Bids = new []{ new VolumePrice { Volume = 50, Price = 9}, new VolumePrice { Volume = 50, Price = 8.5M} }.ToList(),
                }
            }, -99M).Returns(172.5M);
        }
    }
}