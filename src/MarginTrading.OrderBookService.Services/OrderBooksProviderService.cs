using System;
using System.Collections.Generic;
using System.Linq;
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
            var data = await _redisDatabase.HashGetAsync(_dbSettings.OrderBooksCacheKeyPattern, 
                GetKey(exchange, assetPairId));

            if (!data.HasValue)
            {
                throw new Exception($"Order book with exchange: {exchange} and assetPairId {assetPairId} not found.");
            }

            return Deserialize(data);
        }

        public async Task<List<ExternalOrderBook>> GetCurrentOrderBooksAsync(string assetPairId = null)
        {
            var data = await _redisDatabase.HashGetAllAsync(_dbSettings.OrderBooksCacheKeyPattern);
            
            return data
                .Select(x => Deserialize(x.Value))
                .Where(x => string.IsNullOrEmpty(assetPairId) || x.AssetPairId == assetPairId)
                .ToList();
        }

        private string GetKey(string exchangeName, string assetPairId)
        {
            return $"{exchangeName}-{assetPairId}";
        }

        private static ExternalOrderBook Deserialize(string data) 
            => JsonConvert.DeserializeObject<ExternalOrderBook>(data);
    }
}