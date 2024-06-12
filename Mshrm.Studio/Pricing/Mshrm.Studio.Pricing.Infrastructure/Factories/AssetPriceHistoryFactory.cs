using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Infrastructure.Factories
{
    public class AssetPriceHistoryFactory : IAssetPriceHistoryFactory
    {
        /// <summary>
        /// Create an asset price history record
        /// </summary>
        /// <param name="assetPriceId">The asset price that was updated/created</param>
        /// <param name="pricingProviderType">The pricing provider used for import</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="newPrice">The new price</param>
        /// <param name="oldMarketCap">The old market cap</param>
        /// <param name="newMarketCap">The new market cap</param>
        /// <param name="oldVolume">The old volume</param>
        /// <param name="newVolume">The new volume</param>
        /// <returns>Asset price history added</returns>
        public AssetPriceHistory CreateAssetPriceHistory(int assetPriceId, PricingProviderType pricingProviderType, decimal oldPrice, decimal newPrice,
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume)
        {
            return new AssetPriceHistory(assetPriceId, pricingProviderType, oldPrice, newPrice, oldMarketCap, newMarketCap, oldVolume, newVolume);
        }
    }
}
