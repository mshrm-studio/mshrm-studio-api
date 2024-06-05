using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    public class MetalsDevCurrencyPriceProvider : IAssetPriceProvider
    {
        private readonly IMetalsDevService _metalsDevService;
        private readonly ILogger<MetalsDevCurrencyPriceProvider> _logger;
        private readonly MetalsDevServiceOptions _options;

        public MetalsDevCurrencyPriceProvider(IMetalsDevService metalsDevService, IOptions<MetalsDevServiceOptions> options, ILogger<MetalsDevCurrencyPriceProvider> logger)
        {
            _metalsDevService = metalsDevService;
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public async Task<List<ProviderAsset>> GetAssetsAsync()
        {
            var assets = await _metalsDevService.GetAssestAsync();

            return assets.Select(x => new ProviderAsset()
            {
                Symbol = x.Symbol?.ToUpper()?.Trim(),
                MoneySign = x.Symbol?.ToUpper()?.Trim(),
                Name = x.Name
            }).ToList();
        }

        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="assets">The assets to get prices for</param>
        /// <param name="baseAsset">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public async Task<List<PricePair>> GetPricesAsync(List<string> assets, string baseAsset = "USD")
        {
            // Get raw prices
            var prices = await _metalsDevService.GetPricesAsync(baseAsset);

            var selectedPrices = prices.PricePairs.Where(x => assets.Contains(x.Symbol.ToUpper()));

            return selectedPrices.Select(x => new PricePair()
            {
                BaseAsset = baseAsset,
                Asset = x.Symbol.ToUpper(),
                Price = x.Price
            }).ToList();
        }

        /// <summary>
        /// Check if the provider supports the asset
        /// </summary>
        /// <param name="symbol">The asset</param>
        /// <returns>True if supported</returns>
        public async Task<bool> IsAssetSupportedAsync(string symbol)
        {
            var assets = await _metalsDevService.GetAssestAsync();
            var assetSymbols = assets.Select(x => x.Symbol);

            return assetSymbols.Contains(symbol);
        }
    }
}
