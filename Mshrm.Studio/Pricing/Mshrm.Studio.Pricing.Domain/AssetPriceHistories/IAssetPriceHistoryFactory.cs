using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Domain.AssetPriceHistories
{
    public interface IAssetPriceHistoryFactory
    {
        /// <summary>
        /// Create an asset price history record
        /// </summary>
        /// <param name="exchangePricingPairId">The asset price that was updated/created</param>
        /// <param name="pricingProviderType">The pricing provider used for import</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="newPrice">The new price</param>
        /// <param name="oldMarketCap">The old market cap</param>
        /// <param name="newMarketCap">The new market cap</param>
        /// <param name="oldVolume">The old volume</param>
        /// <param name="newVolume">The new volume</param>
        /// <returns>An asset price history record</returns>
        public AssetPriceHistory CreateAssetPriceHistory(int exchangePricingPairId, PricingProviderType pricingProviderType, decimal oldPrice, decimal newPrice,
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume);
    }
}
