using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Repositories.Interfaces
{
    public interface IExchangePricingPairRepository
    {
        /// <summary>
        /// Get the latest exchange pricing pairs
        /// </summary>
        /// <param name="assetIds">The assets to get - all if left null/empty</param>
        /// <param name="pricingProviderType">The provider used to generate price</param>
        /// <param name="assetType">The asset type the price is for</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Pricing pairs for a set of symbols</returns>
        public Task<List<ExchangePricingPair>> GetLatestExchangePricingPairsReadOnlyAsync(List<int>? assetIds, PricingProviderType? pricingProviderType,
            AssetType? assetType, CancellationToken cancellationToken);

        /// <summary>
        /// Add a set of prices
        /// </summary>
        /// <param name="allPairsToAdd">All prices to add</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The added pairs</returns>
        public Task<List<ExchangePricingPair>> CreateExchangePricingPairsAsync(List<ExchangePricingPair> allPairsToAdd, CancellationToken cancellationToken);

        /// <summary>
        /// Update a set of existing pairs
        /// </summary>
        /// <param name="allPairsToUpdate">The pairs to update</param>
        /// <param name="pricingProviderType">The provider used in update</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The updated pairs</returns>
        public Task<List<ExchangePricingPair>> UpdateExchangePricingPairsAsync(List<ExchangePricingPair> allPairsToUpdate, PricingProviderType pricingProviderType,
            CancellationToken cancellationToken);
    }
}
