using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;
using Mshrm.Studio.Pricing.Api.Models.Provider;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;

namespace Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces
{
    public interface IAssetPriceProvider
    {
        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="assets">The assets to get prices for</param>
        /// <param name="baseAsset">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public Task<List<PricePair>> GetPricesAsync(List<string> assets, string baseAsset = "USD");

        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public Task<List<PricingCurrency>> GetAssetsAsync();

        /// <summary>
        /// Check if the provider supports the asset
        /// </summary>
        /// <param name="symbol">The asset</param>
        /// <returns>True if supported</returns>
        Task<bool> IsAssetSupportedAsync(string symbol);
    }
}
