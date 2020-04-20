// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Domain.Abstractions;
using MarginTrading.OrderBookService.Core.Repositories;
using MarginTrading.OrderBookService.Core.Services;

namespace MarginTrading.OrderBookService.Services
{
    public class ExecutionOrderBooksProviderService : IExecutionOrderBooksProviderService
    {
        private readonly IExecutionOrderBookRepository _executionOrderBookRepository;

        public ExecutionOrderBooksProviderService(
            IExecutionOrderBookRepository executionOrderBookRepository)
        {
            _executionOrderBookRepository = executionOrderBookRepository;
        }
        
        public async Task<IOrderExecutionOrderBook> GetAsync(string orderId)
        {
            
            return await _executionOrderBookRepository.GetAsync(orderId);
        }
    }
}