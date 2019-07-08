// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;
using Lykke.Snow.Common.Startup.ApiKey;

namespace MarginTrading.OrderBookService.Core.Settings
{
    [UsedImplicitly]
    public class AppSettings
    {
        public OrderBookServiceSettings OrderBookService { get; set; }
        
        [Optional, CanBeNull]
        public ClientSettings OrderBookServiceClient { get; set; } = new ClientSettings();
    }
}
