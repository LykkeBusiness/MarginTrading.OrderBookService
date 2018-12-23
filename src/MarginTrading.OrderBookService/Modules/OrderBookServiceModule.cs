using System;
using Autofac;
using Common.Log;
using Lykke.Common;
using Lykke.Common.Chaos;
using Lykke.SettingsReader;
using MarginTrading.OrderBookService.Core.Repositories;
using MarginTrading.OrderBookService.Core.Services;
using MarginTrading.OrderBookService.Core.Settings;
using MarginTrading.OrderBookService.Services;
using MarginTrading.OrderBookService.SqlRepositories;
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
            if (_settings.CurrentValue.OrderBookService.Db.StorageMode == StorageMode.Azure)
            {
                //todo implement azure repos before using
            }
            else if (_settings.CurrentValue.OrderBookService.Db.StorageMode == StorageMode.SqlServer)
            {
                builder.RegisterInstance(new ExecutionOrderBookRepository(
                        _settings.CurrentValue.OrderBookService.Db.DataConnString, _log))
                    .As<IExecutionOrderBookRepository>();
            }
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<OrderBooksProviderService>().As<IOrderBooksProviderService>().SingleInstance();
            
            builder.RegisterType<ExecutionOrderBooksProviderService>().As<IExecutionOrderBooksProviderService>().SingleInstance();
            
            builder.RegisterType<ConvertService>()
                .As<IConvertService>()
                .SingleInstance();
            
            
        }

        private void RegisterRedis(ContainerBuilder builder)
        {
            builder.Register(c => ConnectionMultiplexer.Connect(
                    _settings.CurrentValue.OrderBookService.Db.RedisSettings.Configuration))
                .As<IConnectionMultiplexer>()
                .SingleInstance();

            builder.Register(c => c.Resolve<IConnectionMultiplexer>().GetDatabase())
                .As<IDatabase>();
        }
    }
}