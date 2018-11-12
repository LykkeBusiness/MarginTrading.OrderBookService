using JetBrains.Annotations;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.SettingsReader.Attributes;

namespace MarginTrading.OrderBookService.OrderBookBroker
{
    [UsedImplicitly]
    public class Settings : BrokerSettingsBase
    {
        public Db Db { get; set; }
        
        public RabbitMqQueues RabbitMqQueues { get; set; }

        [Optional] 
        public string OrderBooksCacheKeyPattern { get; set; } = "OrderBookService:{0}:{1}";
    }
    
    public static class CacheSettingsExt
    {
        public static string GetOrderBookKey(this Settings settings, string exchangeName, string assetPairId)
        {
            return string.Format(settings.OrderBooksCacheKeyPattern, exchangeName, assetPairId);
        }
    }
    
    [UsedImplicitly]
    public class Db
    {
        public string RedisConfiguration { get; set; }
    }
    
    [UsedImplicitly]
    public class RabbitMqQueues
    {
        public RabbitMqQueueSettings OrderBooks { get; set; }
    }
}
