// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Services;
using MarginTrading.OrderBookService.Core.Settings;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MarginTrading.OrderBookService.Services
{
    public class OrderBooksProviderService : IOrderBooksProviderService
    {
        private readonly IDatabase _redisDatabase;
        private readonly OrderBookServiceSettings _settings;
        private readonly ILog _log;

        public OrderBooksProviderService(
            IDatabase redisDatabase,
            OrderBookServiceSettings settings, 
            ILog log)
        {
            _redisDatabase = redisDatabase;
            _settings = settings;
            _log = log;
        }
        
        public async Task<ExternalOrderBook> GetCurrentOrderBookAsync(string exchange, string assetPairId)
        {
            var data = await _redisDatabase.HashGetAsync(_settings.Db.OrderBooksCacheKeyPattern, 
                GetKey(exchange, assetPairId));

            if (!data.HasValue)
            {
                this._log.WriteWarningAsync(nameof(OrderBooksProviderService), nameof(GetCurrentOrderBookAsync),
                    $"Order book with exchange: {exchange} and assetPairId {assetPairId} not found.");
                return null;
            }

            return Deserialize(data);
        }

        public async Task<List<ExternalOrderBook>> GetCurrentOrderBooksAsync(string assetPairId = null)
        {
            var data = await _redisDatabase.HashGetAllAsync(_settings.Db.OrderBooksCacheKeyPattern);
            
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