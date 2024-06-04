using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Services.Http;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    /// <summary>
    /// Provider PolygonIO
    /// </summary>
    public class PolygonIOCurrencyPriceProvider : IAssetPriceProvider
    {
        private readonly IPolygonIOService _polygonIOService;
        private readonly ILogger<PolygonIOCurrencyPriceProvider> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonIOCurrencyPriceProvider"/> class.
        /// </summary>
        /// <param name="polygonIOService"></param>
        /// <param name="logger"></param>
        public PolygonIOCurrencyPriceProvider(IPolygonIOService polygonIOService, ILogger<PolygonIOCurrencyPriceProvider> logger)
        {
            _polygonIOService = polygonIOService;
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
            var prices = await _polygonIOService.GetPricesAsync(assets, baseAsset);

            return prices.Select(x => new PricePair()
            {
                BaseAsset = baseAsset,
                Asset = x.Symbol,
                Price = x.Close ?? x.Open,
                Volume = x.Volume,
            }).ToList();
        }

        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public async Task<List<PricingCurrency>> GetAssetsAsync()
        {
            var currencies = await _polygonIOService.GetCurrenciesAsync(null);

            return currencies?.Currencies.Select(x => new PricingCurrency()
            {
                MoneySign = x.Ticker,
                Name = x.Name,
                Symbol = x.Ticker
            }).ToList() ?? new List<PricingCurrency>();
        }

        /// <summary>
        /// Check if the provider supports the asset
        /// </summary>
        /// <param name="symbol">The asset</param>
        /// <returns>True if supported</returns>
        public async Task<bool> IsAssetSupportedAsync(string symbol)
        {
            var currencies = await _polygonIOService.GetCurrenciesAsync(symbol);

            return currencies?.Currencies?.Any() ?? false;
        }
    }
}
