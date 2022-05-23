// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.MarginTrading.BrokerBase;
using Lykke.MarginTrading.BrokerBase.Models;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using Lykke.SlackNotifications;
using MarginTrading.OrderBookService.Core.Repositories;
using Polly;
using Polly.Retry;

namespace MarginTrading.OrderBookService.ExecutionOrderBookBroker
{
    public class Application : BrokerApplicationBase<OrderExecutionOrderBookContract>
    {
        private readonly IExecutionOrderBookRepository _executionOrderBookRepository;
        private readonly ILog _log;
        private readonly Settings _settings;

        private AsyncRetryPolicy _retryPolicy;

        public Application(
            IExecutionOrderBookRepository executionOrderBookRepository,
            ILog logger,
            Settings settings, 
            CurrentApplicationInfo applicationInfo,
            ISlackNotificationsSender slackNotificationsSender) 
        : base(logger, slackNotificationsSender, applicationInfo, MessageFormat.MessagePack)
        {
            _executionOrderBookRepository = executionOrderBookRepository;
            _log = logger;
            _settings = settings;
            
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4),
                    TimeSpan.FromSeconds(8),
                }, onRetry: (exception, timespan, retryCount, context) =>
                {
                    _log.WriteWarningAsync(nameof(ExecutionOrderBookBroker), nameof(HandleMessage),
                        $"Cannot save orderBookMessage with order id {context["id"]}, external order id {context["externalOrderId"]}, retryCount {retryCount}",
                        exception);
                });
        }

        protected override BrokerSettingsBase Settings => _settings;
        protected override string ExchangeName => _settings.RabbitMqQueues.ExecutionOrderBooks.ExchangeName;
        public override string RoutingKey => "OrderExecutionOrderBookContract";
        
        protected override Task HandleMessage(OrderExecutionOrderBookContract orderBookMessage)
        {
            var orderBook = orderBookMessage.ToDomain();
            
            return Task.Run(async () =>
            {
                try
                {
                    await _retryPolicy.ExecuteAsync((context) => _executionOrderBookRepository.AddAsync(orderBook),
                        new Context()
                        {
                            {"id", orderBookMessage.OrderId},
                            {"externalOrderId", orderBookMessage.ExternalOrderId}
                        });
                }
                catch (Exception ex)
                {
                    await _log.WriteErrorAsync(nameof(ExecutionOrderBookBroker), nameof(HandleMessage), 
                        orderBook.ToJson(), ex);
                }
            });
        }
    }
}






















