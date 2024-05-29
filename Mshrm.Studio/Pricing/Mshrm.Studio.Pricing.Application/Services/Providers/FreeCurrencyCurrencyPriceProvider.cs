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
    public class FreeCurrencyCurrencyPriceProvider : ICurrencyPriceProvider
    {
        private readonly IFreeCurrencyService _freeCurrencyService;
        private readonly ILogger<FreeCurrencyCurrencyPriceProvider> _logger;

        public FreeCurrencyCurrencyPriceProvider(IFreeCurrencyService freeCurrencyService, ILogger<FreeCurrencyCurrencyPriceProvider> logger) 
        {
            _freeCurrencyService = freeCurrencyService;
            _logger = logger;
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
            var prices = await _freeCurrencyService.GetPricesAsync(baseCurrency);

            return prices.Where(x => currencies.Contains(x.Currency)).Select(x => new PricePair()
            {
                BaseCurrency = baseCurrency,
                Currency = x.Currency,
                Price = x.Price
            }).ToList();
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public async Task<List<PricingCurrency>> GetCurrenciesAsync()
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
        /// Check if the provider supports the currency
        /// </summary>
        /// <param name="symbol">The currency</param>
        /// <returns>True if supported</returns>
        public async Task<bool> IsCurrencySupportedAsync(string symbol)
        {
            // Get currencies
            var currencies = await _freeCurrencyService.GetCurrenciesAsync();

            return currencies.Select(x => x.Symbol == symbol.ToUpper().Trim()).Any();
        }
    }
}
