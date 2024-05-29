using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;
using Mshrm.Studio.Pricing.Api.Models.Provider;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;

namespace Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces
{
    public interface ICurrencyPriceProvider
    {
        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="currencies">The currencies to get prices for</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public Task<List<PricePair>> GetPricesAsync(List<string> currencies, string baseCurrency = "USD");

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public Task<List<PricingCurrency>> GetCurrenciesAsync();

        /// <summary>
        /// Check if the provider supports the currency
        /// </summary>
        /// <param name="symbol">The currency</param>
        /// <returns>True if supported</returns>
        Task<bool> IsCurrencySupportedAsync(string symbol);
    }
}
