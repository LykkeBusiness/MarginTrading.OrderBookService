// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Autofac;
using Common.Log;
using JetBrains.Annotations;
using Lykke.MarginTrading.BrokerBase;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.SettingsReader;
using MarginTrading.OrderBookService.Core.Modules;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;

namespace MarginTrading.OrderBookService.OrderBookBroker
{
    [UsedImplicitly]
    public class Startup : BrokerStartupBase<DefaultBrokerApplicationSettings<Settings>, Settings>
    {
        public Startup(IHostEnvironment env) : base(env)
        {
        }

        protected override string ApplicationName => "OrderBookBroker";

        protected override void RegisterCustomServices(ContainerBuilder builder, 
            IReloadingManager<Settings> settings, ILog log)
        {
            var currentSettings = settings.CurrentValue;
            
            builder.RegisterType<Application>().As<IBrokerApplication>().SingleInstance();

            builder.RegisterType<SystemClock>().As<ISystemClock>().SingleInstance();

            builder.RegisterModule(new RedisModule(currentSettings.Db.RedisConfiguration));
        }
    }
}
