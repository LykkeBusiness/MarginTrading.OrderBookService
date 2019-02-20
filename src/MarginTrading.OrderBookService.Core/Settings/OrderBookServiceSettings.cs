using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.SettingsReader.Attributes;

namespace MarginTrading.OrderBookService.Core.Settings
{
    [UsedImplicitly]
    public class OrderBookServiceSettings
    {
        public DbSettings Db { get; set; }

        [Optional, CanBeNull]
        public ChaosSettings ChaosKitty { get; set; }
        
        [Optional]
        public bool UseSerilog { get; set; }
        
        [Optional]
        public string DefaultExchangeName { get; set; }
    }
}
