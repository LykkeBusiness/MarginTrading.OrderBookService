using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MarginTrading.OrderBookService.Core.Settings
{
    [UsedImplicitly]
    public class DbSettings
    {
        public StorageMode StorageMode { get; set; }
        public string LogsConnString { get; set; }

        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public RedisSettings RedisSettings { get; set; }
        
        [Optional] 
        public string OrderBooksCacheKeyPattern { get; set; } = "OrderBookService:{0}:{1}";
    }
}
