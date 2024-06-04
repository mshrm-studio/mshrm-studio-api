using Mshrm.Studio.Pricing.Api.Models.Cache;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface ITwelveDataService
    {
        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="baseAsset">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public Task<List<PricePair>> GetPricesAsync(string baseAsset = "USD");
    }
}
