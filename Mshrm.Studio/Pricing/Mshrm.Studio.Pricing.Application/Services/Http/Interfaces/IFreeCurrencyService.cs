using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.FreeCurrency;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface IFreeCurrencyService
    {
        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public Task<List<FreeCurrencyPriceResponse>> GetPricesAsync(string baseCurrency = "USD");

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public Task<List<FreeCurrencyCurrencyResponse>> GetCurrenciesAsync();
    }
}
