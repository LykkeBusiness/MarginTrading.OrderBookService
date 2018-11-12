using System;
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
using StackExchange.Redis;

namespace MarginTrading.OrderBookService.OrderBookBroker
{
    public class Application : BrokerApplicationBase<ExternalExchangeOrderbookMessage>
    {
        private readonly IDatabase _redisDatabase;
        private readonly ILog _log;
        private readonly Settings _settings;

        public Application(
            IDatabase redisDatabase,
            ILog logger,
            Settings settings, 
            CurrentApplicationInfo applicationInfo,
            ISlackNotificationsSender slackNotificationsSender) 
        : base(logger, slackNotificationsSender, applicationInfo, MessageFormat.MessagePack)
        {
            _redisDatabase = redisDatabase;
            _log = logger;
            _settings = settings;
        }

        protected override BrokerSettingsBase Settings => _settings;
        protected override string ExchangeName => _settings.RabbitMqQueues.OrderBooks.ExchangeName;
        protected override string RoutingKey => null;

        protected override Task HandleMessage(ExternalExchangeOrderbookMessage orderBookMessage)
        {
            var orderBook = orderBookMessage.ToDomain();
            
            return Task.Run(async () =>
            {
                try
                {
                    await _redisDatabase.StringSetAsync(GetKey(orderBook.ExchangeName, orderBook.AssetPairId), 
                        orderBook.ToJson());
                }
                catch (Exception ex)
                {
                    await _log.WriteErrorAsync(nameof(OrderBookBroker), nameof(HandleMessage), "SwitchThread", ex);
                }
            });
        }

        private string GetKey(string exchangeName, string assetPairId)
        {
            return string.Format(_settings.OrderBooksCacheKeyPattern, exchangeName, assetPairId);
        }
    }
}






















