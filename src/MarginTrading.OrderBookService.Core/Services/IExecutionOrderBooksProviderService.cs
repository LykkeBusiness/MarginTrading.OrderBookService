// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Services
{
    public interface IExecutionOrderBooksProviderService
    {
        [Obsolete("Use method GetByExternalOrderAsync instead.")]
        Task<IOrderExecutionOrderBook> GetAsync(string orderId);
        Task<IOrderExecutionOrderBook> GetByExternalOrderAsync(string externalOrderId);
    }
}