using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.FreeCurrency;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface IFreeCurrencyService
    {
        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="baseAsset">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public Task<List<FreeCurrencyPriceResponse>> GetPricesAsync(string baseAsset = "USD");

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public Task<List<FreeCurrencyCurrencyResponse>> GetCurrenciesAsync();
    }
}
