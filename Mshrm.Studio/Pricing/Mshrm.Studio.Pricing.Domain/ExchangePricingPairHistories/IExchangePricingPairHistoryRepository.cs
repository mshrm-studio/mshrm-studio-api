using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Repositories.Interfaces
{
    public interface IExchangePricingPairHistoryRepository
    {
        /// <summary>
        /// Add history for price updates
        /// </summary>
        /// <param name="exchangePricingPairId">The id of the pair being updated</param>
        /// <param name="newPrice">The new price</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="newMarketCap">The new market cap (if exists)</param>
        /// <param name="oldMarketCap">The old market cap (if exists)</param>
        /// <param name="newVolume">The new volume (if exists)</param>
        /// <param name="oldVolume">The old volume (if exists)</param>
        /// <param name="pricingProviderType">The provider updating</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The history created</returns>
        public Task<ExchangePricingPairHistory> AddHistoryAsync(int exchangePricingPairId, decimal oldPrice, decimal newPrice,
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume, PricingProviderType pricingProviderType,
            CancellationToken cancellationToken);
    }
}
