// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Services;
using NUnit.Framework;

namespace MarginTrading.OrderBookService.Tests
{
    [TestFixture]
    public class SpreadCalculatorTests
    {
        [Test]
        [TestCaseSource(typeof(SpreadCalculatorDataSource), nameof(SpreadCalculatorDataSource.Cases))]
        public decimal Spread_Calculation_Success(OrderExecutionOrderBook orderExecutionOrderBook, decimal volume)
        {
            return SpreadCalculator.CalculateSpread(orderExecutionOrderBook, volume);
        }
    }
}