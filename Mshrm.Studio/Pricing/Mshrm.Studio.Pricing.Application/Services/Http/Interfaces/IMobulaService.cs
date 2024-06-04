using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface IMobulaService
    {
        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="assets">The assets to get prices of</param>
        /// <param name="baseAsset">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public Task<MobulaPriceResponse> GetPricesAsync(List<string> assets, string baseAsset = "USD");

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public Task<MobulaCurrencyResponse> GetCurrenciesAsync(string? symbol);
    }
}
