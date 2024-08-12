// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarginTrading.OrderBookService.Core.Services
{
    public interface ILastNonZeroSpreadService
    {
        Task Update(string assetId, decimal spread);
        
        Task<decimal?> GetSpread(string assetId);
    }
}