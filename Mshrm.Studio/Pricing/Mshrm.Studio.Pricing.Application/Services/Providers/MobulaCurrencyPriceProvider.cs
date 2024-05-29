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
    public class MobulaCurrencyPriceProvider : ICurrencyPriceProvider
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
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public async Task<List<PricingCurrency>> GetCurrenciesAsync()
        {
            var currencies = await _mobulaService.GetCurrenciesAsync(null);

            return currencies.Currencies.Select(x => new PricingCurrency() 
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
            var prices = await _mobulaService.GetPricesAsync(currencies, baseCurrency);

            return prices.PricePairs.Select(x => new PricePair()
            {
                BaseCurrency = baseCurrency,
                Currency = x.Symbol,
                Price = x.Price,
                Volume = x.Volume,
                MarketCap = x.MarketCap
            }).ToList();
        }

        /// <summary>
        /// Check if the provider supports the currency
        /// </summary>
        /// <param name="symbol">The currency</param>
        /// <returns>True if supported</returns>
        public async Task<bool> IsCurrencySupportedAsync(string symbol)
        {
            return (await _mobulaService.GetCurrenciesAsync(symbol)) != null;
        }
    }
}
