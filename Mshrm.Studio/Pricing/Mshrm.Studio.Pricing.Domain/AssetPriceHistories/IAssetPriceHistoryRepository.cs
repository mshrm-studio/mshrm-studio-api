using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Pricing.Domain.AssetPriceHistories
{
    public interface IAssetPriceHistoryRepository
    {
        /// <summary>
        /// Add history for price updates
        /// </summary>
        /// <param name="assetPriceId">The id of the asset price being updated</param>
        /// <param name="newPrice">The new price</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="newMarketCap">The new market cap (if exists)</param>
        /// <param name="oldMarketCap">The old market cap (if exists)</param>
        /// <param name="newVolume">The new volume (if exists)</param>
        /// <param name="oldVolume">The old volume (if exists)</param>
        /// <param name="pricingProviderType">The provider updating</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The history created</returns>
        public Task<AssetPriceHistory> AddAssetPriceHistoryAsync(int assetPriceId, decimal oldPrice, decimal newPrice,
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume, PricingProviderType pricingProviderType,
            CancellationToken cancellationToken);

        /// <summary>
        /// Get the paged history prices
        /// </summary>
        /// <param name="assetId">The asset the history is for</param>
        /// <param name="pricingProviderType">The provider updating price (creating row)</param>
        /// <param name="page">The page</param>
        /// <param name="sortOrder">The sort order</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Page of price history</returns>
        public Task<PagedResult<AssetPriceHistory>> GetPagedHistory(int assetId, PricingProviderType? pricingProviderType,
             Page page, SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
