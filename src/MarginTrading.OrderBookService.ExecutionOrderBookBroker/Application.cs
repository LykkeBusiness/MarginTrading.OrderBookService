// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Common;
using Lykke.MarginTrading.BrokerBase;
using Lykke.MarginTrading.BrokerBase.Messaging;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using Lykke.Snow.Common.Correlation.RabbitMq;

using MarginTrading.OrderBookService.Core.Repositories;

using Microsoft.Extensions.Logging;

using Polly;
using Polly.Retry;

namespace MarginTrading.OrderBookService.ExecutionOrderBookBroker
{
    public class Application : BrokerApplicationBase<OrderExecutionOrderBookContract>
    {
        private readonly IExecutionOrderBookRepository _executionOrderBookRepository;
        private readonly Settings _settings;
        private readonly AsyncRetryPolicy _retryPolicy;

        public Application(
            RabbitMqCorrelationManager correlationManager,
            IExecutionOrderBookRepository executionOrderBookRepository,
            Settings settings,
            CurrentApplicationInfo applicationInfo,
            IMessagingComponentFactory<OrderExecutionOrderBookContract> messagingComponentFactory,
            ILoggerFactory loggerFactory
        )
            : base(
                correlationManager,
                loggerFactory,
                applicationInfo,
                messagingComponentFactory)
        {
            _executionOrderBookRepository = executionOrderBookRepository;
            _settings = settings;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    new[]
                    {
                        TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4),
                        TimeSpan.FromSeconds(8),
                    },
                    onRetry: (
                        exception,
                        timespan,
                        retryCount,
                        context) =>
                    {
                        Logger.LogWarning(
                            exception,
                            "Cannot save orderBookMessage with order id {OrderId}, external order id {ExternalOrderId}, retryCount {RetryCount}",
                            context["id"],
                            context["externalOrderId"],
                            retryCount);
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
                        new Context
                        {
                            {"id", orderBookMessage.OrderId},
                            {"externalOrderId", orderBookMessage.ExternalOrderId}
                        });
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Failed to save order book: {OrderBook}", orderBook.ToJson());
                }
            });
        }
    }
}






















