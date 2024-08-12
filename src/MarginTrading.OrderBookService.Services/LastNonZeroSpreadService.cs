// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MarginTrading.OrderBookService.Core.Services;

using StackExchange.Redis;

namespace MarginTrading.OrderBookService.Services
{
    public class LastNonZeroSpreadService: ILastNonZeroSpreadService
    {
        private const string RedisLastNonZeroSpreadKeyFmt = "orderbook:asset:{0}:last-non-zero-spread";
        private readonly IConnectionMultiplexer _redis;

        public LastNonZeroSpreadService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task Update(string assetId, decimal spread)
        {
            var key = GetRedisLastNonZeroSpreadKey(assetId);
            await _redis.GetDatabase().StringSetAsync(key, spread.ToString());
        }

        public async Task<decimal?> GetSpread(string assetId)
        {
            var key = GetRedisLastNonZeroSpreadKey(assetId);
            var serialized = await _redis.GetDatabase().StringGetAsync(key);

            if (!serialized.HasValue) return null;
            
            return decimal.Parse(serialized);
        }

        private static RedisKey GetRedisLastNonZeroSpreadKey(string assetId) => string.Format(RedisLastNonZeroSpreadKeyFmt, assetId);
    }
}