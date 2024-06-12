using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Repositories.Interfaces
{
    public interface IAssetPriceRepository
    {
        /// <summary>
        /// Get the latest asset prices
        /// </summary>
        /// <param name="assetIds">The assets to get - all if left null/empty</param>
        /// <param name="pricingProviderType">The provider used to generate price</param>
        /// <param name="assetType">The asset type the price is for</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Asset prices for a set of symbols</returns>
        public Task<List<AssetPrice>> GetLatestAssetPricesReadOnlyAsync(List<int>? assetIds, PricingProviderType? pricingProviderType,
            AssetType? assetType, CancellationToken cancellationToken);

        /// <summary>
        /// Add a set of asset prices
        /// </summary>
        /// <param name="allAssetPricesToAdd">All prices to add</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The added pairs</returns>
        public Task<List<AssetPrice>> CreateAssetPricesAsync(List<AssetPrice> allAssetPricesToAdd, CancellationToken cancellationToken);

        /// <summary>
        /// Update a set of existing asset prices
        /// </summary>
        /// <param name="allAssetPricesToUpdate">The pairs to update</param>
        /// <param name="pricingProviderType">The provider used in update</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The updated pairs</returns>
        public Task<List<AssetPrice>> UpdateAssetPricesAsync(List<AssetPrice> allAssetPricesToUpdate, PricingProviderType pricingProviderType,
            CancellationToken cancellationToken);
    }
}
