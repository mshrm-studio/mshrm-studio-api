using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;
using Mshrm.Studio.Pricing.Api.Models.HttpService.PolygonIO;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface IPolygonIOService
    {
        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="currencies">The assets to get</param>
        /// <param name="baseCurrency">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public Task<List<PolygonIOPriceResponse>> GetPricesAsync(List<string> assets, string baseAsset = "USD");

        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="symbol">The asset symbol to get</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public Task<PolygonIOPriceResponse> GetPriceAsync(string symbol);

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public Task<PolygonIOCurrencyResponse> GetCurrenciesAsync(string? symbol);
    }
}
