// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain;

namespace MarginTrading.OrderBookService.Core.Services
{
    public interface IOrderBooksProviderService
    {   
        Task<ExternalOrderBook> GetCurrentOrderBookAsync(string exchange, string assetPairId);
        
        Task<List<ExternalOrderBook>> GetCurrentOrderBooksAsync(string assetPairId = null);
    }
}