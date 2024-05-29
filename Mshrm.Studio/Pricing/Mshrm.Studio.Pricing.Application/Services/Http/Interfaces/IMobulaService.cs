using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface IMobulaService
    {
        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="currencies">The currencies to get prices of</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public Task<MobulaPriceResponse> GetPricesAsync(List<string> currencies, string baseCurrency = "USD");

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public Task<MobulaCurrencyResponse> GetCurrenciesAsync(string? symbol);
    }
}
