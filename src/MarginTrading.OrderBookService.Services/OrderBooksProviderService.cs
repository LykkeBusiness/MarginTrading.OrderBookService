using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Services;
using MarginTrading.OrderBookService.Core.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MarginTrading.OrderBookService.Services
{
    public class OrderBooksProviderService : IOrderBooksProviderService
    {
        private readonly IDatabase _redisDatabase;
        private readonly DbSettings _dbSettings;

        public OrderBooksProviderService(
            IDatabase redisDatabase,
            DbSettings dbSettings)
        {
            _redisDatabase = redisDatabase;
            _dbSettings = dbSettings;
        }
        
        public async Task<ExternalOrderBook> GetCurrentOrderBookAsync(string exchange, string assetPairId)
        {
            var data = await _redisDatabase.StringGetAsync(GetKey(exchange, assetPairId));

            if (!data.HasValue)
            {
                throw new Exception($"Order book with exchange: {exchange} and assetPairId {assetPairId} not found.");
            }

            return JsonConvert.DeserializeObject<ExternalOrderBook>(data);
        }

        private string GetKey(string exchangeName, string assetPairId)
        {
            return string.Format(_dbSettings.OrderBooksCacheKeyPattern, exchangeName, assetPairId);
        }
    }
}