using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;
using Mshrm.Studio.Pricing.Api.Models.HttpService.PolygonIO;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface IPolygonIOService
    {
        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="currencies">The currencies to get</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public Task<List<PolygonIOPriceResponse>> GetPricesAsync(List<string> currencies, string baseCurrency = "USD");

        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="symbol">The currency symbol to get</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public Task<PolygonIOPriceResponse> GetPriceAsync(string symbol);

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public Task<PolygonIOCurrencyResponse> GetCurrenciesAsync(string? symbol);
    }
}
