using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;
using System.Diagnostics;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    public class MobulaCurrencyPriceProvider : IAssetPriceProvider
    {
        private readonly IMobulaService _mobulaService;
        private readonly ILogger<MobulaCurrencyPriceProvider> _logger;
        private readonly MobulaServiceOptions _options;

        public MobulaCurrencyPriceProvider(IMobulaService mobulaService, IOptions<MobulaServiceOptions> options, ILogger<MobulaCurrencyPriceProvider> logger)
        {
            _mobulaService = mobulaService;
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public async Task<List<ProviderAsset>> GetAssetsAsync()
        {
            var currencies = await _mobulaService.GetCurrenciesAsync(null);

            return currencies.Currencies.Select(x => new ProviderAsset() 
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
            var prices = await _mobulaService.GetPricesAsync(assets, baseAsset);

            return prices.PricePairs.Select(x => new PricePair()
            {
                BaseAsset = baseAsset,
                Asset = x.Symbol,
                Price = x.Price,
                Volume = x.Volume,
                MarketCap = x.MarketCap
            }).ToList();
        }

        /// <summary>
        /// Check if the provider supports the asset
        /// </summary>
        /// <param name="symbol">The asset</param>
        /// <returns>True if supported</returns>
        public async Task<bool> IsAssetSupportedAsync(string symbol)
        {
            return (await _mobulaService.GetCurrenciesAsync(symbol)) != null;
        }
    }
}
