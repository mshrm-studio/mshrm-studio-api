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
    public class TwelveDataCurrencyPriceProvider : ICurrencyPriceProvider
    {
        private readonly ITwelveDataService _twelveDataService;
        private readonly ILogger<TwelveDataCurrencyPriceProvider> _logger;

        public TwelveDataCurrencyPriceProvider(ITwelveDataService twelveDataService, ILogger<TwelveDataCurrencyPriceProvider> logger) 
        {
            _twelveDataService = twelveDataService;
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
            return await _twelveDataService.GetPricesAsync(baseCurrency);
        }


        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public async Task<List<PricingCurrency>> GetCurrenciesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCurrencySupportedAsync(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
