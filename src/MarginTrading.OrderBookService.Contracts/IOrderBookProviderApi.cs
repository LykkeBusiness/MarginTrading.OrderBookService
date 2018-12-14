using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Lykke.MarginTrading.OrderBookService.Contracts
{
    /// <summary>
    /// API to retrieve current order books
    /// </summary>
    [PublicAPI]
    public interface IOrderBookProviderApi
    {
        /// <summary>
        /// Get current order book for <paramref name="exchange"/> and <paramref name="assetPair"/>.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="assetPair"></param>
        /// <returns></returns>
        [ItemCanBeNull]
        [Get("/api/orderbookprovider/GetOrderBook")]
        Task<ExternalOrderBookContract> GetOrderBook([NotNull] string exchange, [NotNull] string assetPair);
        
        /// <summary>
        /// Get all current order books.
        /// </summary>
        /// <returns></returns>
        [Get("/api/orderbookprovider/GetOrderBooks")]
        Task<List<ExternalOrderBookContract>> GetOrderBooks();

        /// <summary>
        /// Get trade execution order book for <paramref name="orderId"/>.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Get("/api/orderbookprovider/GetExecutionOrderBook")]
        Task<OrderExecutionOrderBookContract> GetExecutionOrderBook(string orderId);
    }
}