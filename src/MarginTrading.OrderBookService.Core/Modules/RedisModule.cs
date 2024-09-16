// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Autofac;

using StackExchange.Redis;

namespace MarginTrading.OrderBookService.Core.Modules
{
    public class RedisModule : Module
    {
        private readonly string _redisConfiguration;

        public RedisModule(string redisConfiguration)
        {
            _redisConfiguration = redisConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => ConnectionMultiplexer.Connect(_redisConfiguration))
                .As<IConnectionMultiplexer>()
                .SingleInstance();
        }
    }
}