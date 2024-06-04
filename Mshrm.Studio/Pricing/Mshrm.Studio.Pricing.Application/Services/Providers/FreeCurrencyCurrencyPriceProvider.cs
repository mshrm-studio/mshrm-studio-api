using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Models.Provider;
using Mshrm.Studio.Pricing.Api.Services.Http.Bases;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;
using Mshrm.Studio.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    public class FreeCurrencyCurrencyPriceProvider : IAssetPriceProvider
    {
        private readonly IFreeCurrencyService _freeCurrencyService;
        private readonly ILogger<FreeCurrencyCurrencyPriceProvider> _logger;

        public FreeCurrencyCurrencyPriceProvider(IFreeCurrencyService freeCurrencyService, ILogger<FreeCurrencyCurrencyPriceProvider> logger) 
        {
            _freeCurrencyService = freeCurrencyService;
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
            // Get raw prices
            var prices = await _freeCurrencyService.GetPricesAsync(baseAsset);

            return prices.Where(x => assets.Contains(x.Currency)).Select(x => new PricePair()
            {
                BaseAsset = baseAsset,
                Asset = x.Currency,
                Price = x.Price
            }).ToList();
        }

        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public async Task<List<PricingCurrency>> GetAssetsAsync()
        {
            // Get raw currencies
            var currencies = await _freeCurrencyService.GetCurrenciesAsync();

            return currencies.Select(x => new PricingCurrency()
            {
                MoneySign = x.MoneySign,
                Name = x.Name,
                Symbol = x.Symbol,
            }).ToList();
        }

        /// <summary>
        /// Check if the provider supports the asset
        /// </summary>
        /// <param name="symbol">The asset</param>
        /// <returns>True if supported</returns>
        public async Task<bool> IsAssetSupportedAsync(string symbol)
        {
            // Get assets
            var currencies = await _freeCurrencyService.GetCurrenciesAsync();

            return currencies.Select(x => x.Symbol == symbol.ToUpper().Trim()).Any();
        }
    }
}
