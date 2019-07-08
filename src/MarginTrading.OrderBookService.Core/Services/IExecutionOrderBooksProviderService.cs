// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;

namespace MarginTrading.OrderBookService.Core.Services
{
    public interface IExecutionOrderBooksProviderService
    {
        Task<IOrderExecutionOrderBook> GetAsync(string orderId);
    }
}