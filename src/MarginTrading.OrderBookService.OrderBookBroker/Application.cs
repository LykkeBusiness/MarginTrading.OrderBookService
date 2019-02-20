using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.MarginTrading.BrokerBase;
using Lykke.MarginTrading.BrokerBase.Models;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.SlackNotifications;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderbookAggregator.Contracts.Messages;
using Microsoft.Extensions.Internal;
using StackExchange.Redis;

namespace MarginTrading.OrderBookService.OrderBookBroker
{
    public class Application : BrokerApplicationBase<ExternalExchangeOrderbookMessage>
    {
        private readonly IDatabase _redisDatabase;
        private readonly ISystemClock _systemClock;
        private readonly ILog _log;
        private readonly Settings _settings;
        private readonly ConcurrentDictionary<string, DateTime> _lastMessageTimes = 
            new ConcurrentDictionary<string, DateTime>();

        public Application(
            IDatabase redisDatabase,
            ISystemClock systemClock,
            ILog logger,
            Settings settings, 
            CurrentApplicationInfo applicationInfo,
            ISlackNotificationsSender slackNotificationsSender) 
        : base(logger, slackNotificationsSender, applicationInfo, MessageFormat.MessagePack)
        {
            _redisDatabase = redisDatabase;
            _systemClock = systemClock;
            _log = logger;
            _settings = settings;
        }

        protected override BrokerSettingsBase Settings => _settings;
        protected override string ExchangeName => _settings.RabbitMqQueues.OrderBooks.ExchangeName;
        protected override string RoutingKey => null;
        
        protected override Task HandleMessage(ExternalExchangeOrderbookMessage orderBookMessage)
        {
            var messageTime = DateTime.UtcNow;
            var key = GetKey(orderBookMessage.ExchangeName, orderBookMessage.AssetPairId);
            var previousTime = _lastMessageTimes.TryGetValue(key, out var previousTimeExtracted)
                ? previousTimeExtracted
                : DateTime.MinValue;

            if (!_settings.OrderBookThrottlingRateThreshold.HasValue
                || messageTime.Subtract(previousTime).TotalSeconds > (1 / _settings.OrderBookThrottlingRateThreshold))
            {
                _lastMessageTimes.AddOrUpdate(key, messageTime, (k, v) => messageTime);
                return HandleMessageWithoutThrottling(orderBookMessage);
            }
            
            return Task.CompletedTask;
        }

        private Task HandleMessageWithoutThrottling(ExternalExchangeOrderbookMessage orderBookMessage)
        {
            var orderBook = orderBookMessage.ToDomain(_systemClock.UtcNow.UtcDateTime);
            
            return Task.Run(async () =>
            {
                try
                {
                    await _redisDatabase.HashSetAsync(_settings.OrderBooksCacheKeyPattern, new []
                    {
                        new HashEntry(GetKey(orderBook.ExchangeName, orderBook.AssetPairId), orderBook.ToJson()), 
                    });
                }
                catch (Exception ex)
                {
                    await _log.WriteErrorAsync(nameof(OrderBookBroker), nameof(HandleMessageWithoutThrottling), "SwitchThread", ex);
                }
            });
        }

        private string GetKey(string exchangeName, string assetPairId)
        {
            return $"{exchangeName}-{assetPairId}";
        }
    }
}






















