using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    public class MetalsDevCurrencyPriceProvider : ICurrencyPriceProvider
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
        /// Get all currencies (assets in this case)
        /// </summary>
        /// <returns>Currencies</returns>
        public async Task<List<PricingCurrency>> GetCurrenciesAsync()
        {
            var assets = await _metalsDevService.GetAssestAsync();

            return assets.Select(x => new PricingCurrency()
            {
                Symbol = x.Symbol?.ToUpper()?.Trim(),
                MoneySign = x.Symbol?.ToUpper()?.Trim(),
                Name = x.Name
            }).ToList();
        }

        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="currencies">The currencies to get prices for</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public async Task<List<PricePair>> GetPricesAsync(List<string> currencies, string baseCurrency = "USD")
        {
            // Get raw prices
            var prices = await _metalsDevService.GetPricesAsync(baseCurrency);

            var selectedPrices = prices.PricePairs.Where(x => currencies.Contains(x.Symbol.ToUpper()));

            return selectedPrices.Select(x => new PricePair()
            {
                BaseCurrency = baseCurrency,
                Currency = x.Symbol.ToUpper(),
                Price = x.Price
            }).ToList();
        }

        /// <summary>
        /// Check if the provider supports the currency
        /// </summary>
        /// <param name="symbol">The currency</param>
        /// <returns>True if supported</returns>
        public async Task<bool> IsCurrencySupportedAsync(string symbol)
        {
            var assets = await _metalsDevService.GetAssestAsync();
            var assetSymbols = assets.Select(x => x.Symbol);

            return assetSymbols.Contains(symbol);
        }
    }
}
