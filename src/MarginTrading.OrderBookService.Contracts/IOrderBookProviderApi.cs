using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Lykke.MarginTrading.OrderBookService.Contracts
{
    [PublicAPI]
    public interface IOrderBookProviderApi
    {
        [ItemCanBeNull]
        [Get("/api/orderbookprovider/GetOrderBook")]
        Task<ExternalOrderBookContract> GetOrderBook([NotNull] string exchange, [NotNull] string assetPair);
        
        [Get("/api/orderbookprovider/GetOrderBooks")]
        Task<List<ExternalOrderBookContract>> GetOrderBooks();
    }
}