// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Autofac;
using JetBrains.Annotations;
using Lykke.MarginTrading.BrokerBase;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.SettingsReader;
using Lykke.SettingsReader.SettingsTemplate;

using MarginTrading.OrderBookService.Core.Modules;
using MarginTrading.OrderBookService.Core.Services;
using MarginTrading.OrderBookService.OrderBookBroker.ExternalContracts;
using MarginTrading.OrderBookService.Services;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;

namespace MarginTrading.OrderBookService.OrderBookBroker
{
    [UsedImplicitly]
    public class Startup : BrokerStartupBase<DefaultBrokerApplicationSettings<Settings>, Settings>
    {
        public Startup(IHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
        }

        protected override string ApplicationName => "OrderBookBroker";

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSettingsTemplateGenerator();
        }

        protected override void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapSettingsTemplate();
        }

        protected override void RegisterCustomServices(ContainerBuilder builder, IReloadingManager<Settings> settings)
        {
            var currentSettings = settings.CurrentValue;

            builder.AddMessagePackBrokerMessagingFactory<ExternalExchangeOrderbookMessage>();
            builder.RegisterType<Application>().As<IBrokerApplication>().SingleInstance();
            builder.RegisterType<SystemClock>().As<ISystemClock>().SingleInstance();
            builder.RegisterType<LastNonZeroSpreadService>()
                .As<ILastNonZeroSpreadService>()
                .SingleInstance();
            builder.RegisterModule(new RedisModule(currentSettings.Db.RedisConfiguration));
        }
    }
}