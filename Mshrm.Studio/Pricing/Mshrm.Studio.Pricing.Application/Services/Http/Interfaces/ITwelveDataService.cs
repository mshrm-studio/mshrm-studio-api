using Mshrm.Studio.Pricing.Api.Models.Cache;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface ITwelveDataService
    {
        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public Task<List<PricePair>> GetPricesAsync(string baseCurrency = "USD");
    }
}
