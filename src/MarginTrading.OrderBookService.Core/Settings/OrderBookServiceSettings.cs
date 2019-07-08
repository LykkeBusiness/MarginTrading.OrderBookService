﻿// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

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
    }
}
