// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Autofac;
using Common.Log;
using Lykke.Common;
using Lykke.Common.Chaos;
using Lykke.SettingsReader;
using MarginTrading.OrderBookService.Core.Repositories;
using MarginTrading.OrderBookService.Core.Services;
using MarginTrading.OrderBookService.Services;
using MarginTrading.OrderBookService.SqlRepositories;
using MarginTrading.OrderBookService.Settings;
using Microsoft.Extensions.Internal;
using StackExchange.Redis;

namespace MarginTrading.OrderBookService.Modules
{
    internal class OrderBookServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public OrderBookServiceModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings.Nested(s => s.OrderBookService)).SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue.OrderBookService).SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue.OrderBookService.Db).SingleInstance();
            
            builder.RegisterInstance(_log).As<ILog>().SingleInstance();
            builder.RegisterType<SystemClock>().As<ISystemClock>().SingleInstance();
            
            builder.RegisterType<ThreadSwitcherToNewTask>()
                .As<IThreadSwitcher>()
                .SingleInstance();
            
            builder.RegisterChaosKitty(_settings.CurrentValue.OrderBookService.ChaosKitty);

            RegisterServices(builder);
            RegisterRedis(builder);
            RegisterRepositories(builder);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            if (_settings.CurrentValue.OrderBookService.Db.StorageMode == StorageMode.SqlServer)
            {
                builder.RegisterType<ExecutionOrderBookRepository>()
                    .WithParameter(TypedParameter.From(_settings.CurrentValue.OrderBookService.Db.DataConnString))
                    .As<IExecutionOrderBookRepository>()
                    .SingleInstance();
            }
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<OrderBooksProviderService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.OrderBookService.Db.OrderBooksCacheKeyPattern))
                .As<IOrderBooksProviderService>()
                .SingleInstance();
            
            builder.RegisterType<ExecutionOrderBooksProviderService>()
                .As<IExecutionOrderBooksProviderService>()
                .SingleInstance();
        }

        private void RegisterRedis(ContainerBuilder builder)
        {
            builder.Register(c => ConnectionMultiplexer.Connect(
                    _settings.CurrentValue.OrderBookService.Db.RedisSettings.Configuration))
                .As<IConnectionMultiplexer>()
                .SingleInstance();
        }
    }
}