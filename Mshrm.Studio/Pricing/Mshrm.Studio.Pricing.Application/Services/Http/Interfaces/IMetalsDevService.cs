using Mshrm.Studio.Pricing.Api.Models.HttpService.MetalsDev;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Http.Interfaces
{
    public interface IMetalsDevService
    {
        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="unit">The unit to get price of</param>
        /// <param name="unit">The weight</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public Task<MetalsDevPriceResponse> GetPricesAsync(string baseAsset = "USD", string unit = "toz");

        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public Task<List<MetalsDevAsset>> GetAssestAsync();
    }
}
