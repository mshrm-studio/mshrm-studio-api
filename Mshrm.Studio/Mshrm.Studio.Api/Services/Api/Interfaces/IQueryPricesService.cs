using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Prices;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryPricesService
    {
        /// <summary>
        /// Get the latest prices for supported assets
        /// </summary>
        /// <param name="pricingProviderType">An optional pricing provider</param>
        /// <param name="assetType">An optional asset type</param>
        /// <param name="baseAsset">An optional base asset ie. output to what</param>
        /// <param name="symbols">Filter by supported assets</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A list of latest prices</returns>
        Task<List<AssetPriceDto>> GetLatestPricesAsync(PricingProviderType? pricingProviderType, AssetType? assetType, string baseAsset, List<string>? symbols, CancellationToken cancellationToken);
    }
}
