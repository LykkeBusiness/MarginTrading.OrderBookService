// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Autofac;
using JetBrains.Annotations;
using Lykke.MarginTrading.BrokerBase;
using Lykke.MarginTrading.BrokerBase.Models;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using Lykke.SettingsReader;
using Lykke.SettingsReader.SettingsTemplate;
using MarginTrading.OrderBookService.Core.Repositories;
using MarginTrading.OrderBookService.SqlRepositories;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarginTrading.OrderBookService.ExecutionOrderBookBroker
{
    [UsedImplicitly]
    public class Startup : BrokerStartupBase<DefaultBrokerApplicationSettings<Settings>, Settings>
    {
        public Startup(IHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
        }

        protected override string ApplicationName => "ExecutionOrderBookBroker";

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSettingsTemplateGenerator();
        }

        protected override void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.AddSettingsTemplateEndpoint();
        }

        protected override void RegisterCustomServices(ContainerBuilder builder,
            IReloadingManager<Settings> settings)
        {
            builder.AddMessagePackBrokerMessagingFactory<OrderExecutionOrderBookContract>();
            builder.RegisterType<Application>().As<IBrokerApplication>().SingleInstance();

            if (settings.CurrentValue.Db.StorageMode == StorageMode.SqlServer)
            {
                builder.RegisterType<ExecutionOrderBookRepository>()
                    .WithParameter(TypedParameter.From(settings.CurrentValue.Db.ConnString))
                    .As<IExecutionOrderBookRepository>()
                    .SingleInstance();
            }
        }
    }
}