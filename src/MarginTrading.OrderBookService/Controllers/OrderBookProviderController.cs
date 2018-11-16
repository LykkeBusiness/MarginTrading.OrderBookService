using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.MarginTrading.OrderBookService.Contracts;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarginTrading.OrderBookService.Controllers
{
    /// <summary>
    /// Controller to retrieve current order books
    /// </summary>
    [Route("api/[controller]")]
    public class OrderBookProviderController : Controller, IOrderBookProviderApi
    {
        private readonly IOrderBooksProviderService _orderBooksProviderService;

        public OrderBookProviderController(IOrderBooksProviderService orderBooksProviderService)
        {
            _orderBooksProviderService = orderBooksProviderService;
        }

        /// <summary>
        /// Get current order book for <paramref name="exchange"/> and <paramref name="assetPair"/>.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="assetPair"></param>
        /// <returns></returns>
        [HttpGet("GetOrderBook")]
        public async Task<ExternalOrderBookContract> GetOrderBook(string exchange, string assetPair)
        {
            var orderBook = await _orderBooksProviderService.GetCurrentOrderBookAsync(exchange, assetPair);
            
            return orderBook.ToContract();
        }

        /// <summary>
        /// Get all current order books.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrderBooks")]
        public async Task<List<ExternalOrderBookContract>> GetOrderBooks()
        {
            var orderBook = await _orderBooksProviderService.GetCurrentOrderBooksAsync();
            
            return orderBook.Select(x => x.ToContract()).ToList();
        }
    }
}