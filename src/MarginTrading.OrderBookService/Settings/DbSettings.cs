// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MarginTrading.OrderBookService.Settings
{
    [UsedImplicitly]
    public class DbSettings
    {
        public StorageMode StorageMode { get; set; }
        public string LogsConnString { get; set; }
        public string DataConnString { get; set; }

        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public RedisSettings RedisSettings { get; set; }
        
        [Optional] 
        public string OrderBooksCacheKeyPattern { get; set; } = "OrderBookService:{0}:{1}";
    }
}
