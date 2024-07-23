// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common;

using Lykke.MarginTrading.OrderBookService.Contracts;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using MarginTrading.OrderBookService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarginTrading.OrderBookService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LastNonZeroSpreadController : Controller, ILastNonZeroSpreadApi
    {
        private readonly ILastNonZeroSpreadService _lastNonZeroSpreadService;

        public LastNonZeroSpreadController(ILastNonZeroSpreadService lastNonZeroSpreadService)
        {
            _lastNonZeroSpreadService = lastNonZeroSpreadService;
        }

        [HttpGet]
        public async Task<Dictionary<string, decimal>> GetLastNonZeroSpreadByAssetIds(IEnumerable<string> assetIds)
        {
            return (await assetIds
                .SelectAsync(async assetId => new
                {
                    AssetId = assetId,
                    Spread = await _lastNonZeroSpreadService.GetSpread(assetId)
                }))
                .Where(x => x.Spread.HasValue)
                .ToDictionary(x => x.AssetId, x => x.Spread.Value);
        }
    }
}