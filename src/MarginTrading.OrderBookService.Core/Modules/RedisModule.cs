using Autofac;
using MarginTrading.OrderBookService.Core.Settings;
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

            builder.Register(c => c.Resolve<IConnectionMultiplexer>().GetDatabase())
                .As<IDatabase>();
        }
    }
}