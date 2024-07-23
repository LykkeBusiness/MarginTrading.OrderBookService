// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;
using Refit;

namespace Lykke.MarginTrading.OrderBookService.Contracts
{
    [PublicAPI]
    public interface ILastNonZeroSpreadApi
    {
        [Get("/api/lastnonzerospread")]
        Task<Dictionary<string, decimal>> GetLastNonZeroSpreadByAssetIds(IEnumerable<string> assetIds);
    }
}