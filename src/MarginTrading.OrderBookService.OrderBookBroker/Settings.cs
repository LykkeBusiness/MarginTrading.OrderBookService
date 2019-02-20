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
        public int? OrderBookThrottlingRateThreshold { get; set; }

        [Optional] 
        public string OrderBooksCacheKeyPattern { get; set; } = "OrderBookService:OrderBooks";
        
        [Optional]
        public string DefaultExchangeName { get; set; }
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
