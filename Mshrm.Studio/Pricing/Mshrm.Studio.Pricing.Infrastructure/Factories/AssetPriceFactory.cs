using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Domain.AssetPrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Infrastructure.Factories
{
    public class AssetPriceFactory : IAssetPriceFactory
    {
        /// <summary>
        /// Create a new asset price
        /// </summary>
        /// <param name="baseAssetId">The base asset</param>
        /// <param name="assetId">The asset to create price for</param>
        /// <param name="price">The price against base asset</param>
        /// <param name="marketCap">The market cap</param>
        /// <param name="volume">The volume</param>
        /// <param name="pricingProviderType">A pricing provider type (imported with)</param>
        /// <returns>A price for a asset</returns>
        public AssetPrice CreateAssetPrice(int baseAssetId, int assetId, decimal price, decimal? marketCap, decimal? volume, PricingProviderType pricingProviderType)
        {
            return new AssetPrice(baseAssetId, assetId, price, marketCap, volume, pricingProviderType);
        }
    }
}
