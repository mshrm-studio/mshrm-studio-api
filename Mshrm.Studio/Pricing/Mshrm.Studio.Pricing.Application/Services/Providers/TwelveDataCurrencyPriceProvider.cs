using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Models.Provider;
using Mshrm.Studio.Pricing.Api.Services.Http.Bases;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    public class TwelveDataCurrencyPriceProvider : IAssetPriceProvider
    {
        private readonly ITwelveDataService _twelveDataService;
        private readonly ILogger<TwelveDataCurrencyPriceProvider> _logger;

        public TwelveDataCurrencyPriceProvider(ITwelveDataService twelveDataService, ILogger<TwelveDataCurrencyPriceProvider> logger) 
        {
            _twelveDataService = twelveDataService;
            _logger = logger;
        }

        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="assets">The assets to get prices for</param>
        /// <param name="baseAsset">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public async Task<List<PricePair>> GetPricesAsync(List<string> assets, string baseAsset = "USD")
        {
            return await _twelveDataService.GetPricesAsync(baseAsset);
        }


        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public async Task<List<ProviderAsset>> GetAssetsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsAssetSupportedAsync(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
