using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.FreeCurrency;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Models.Provider;
using Mshrm.Studio.Pricing.Api.Services.Http.Bases;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    public class FreeCurrencyService : BaseHttpService, IFreeCurrencyService
    {
        private readonly FreeCurrencyOptions _options;
        private readonly ILogger<FreeCurrencyService> _logger;

        public FreeCurrencyService(HttpClient client, IOptions<FreeCurrencyOptions> options, ILogger<FreeCurrencyService> logger) : base(client) 
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public async Task<List<FreeCurrencyPriceResponse>> GetPricesAsync(string baseCurrency = "USD")
        {
            // Get raw prices
            var prices = await base.GetAsync<object>(
                $"{_options.Endpoint}/v1/latest?apikey={_options.ApiKey}&base_currency={baseCurrency}&currencies=", false);

            // Process into response
            return GetPricePairs(baseCurrency, JObject.FromObject(prices));
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public async Task<List<FreeCurrencyCurrencyResponse>> GetCurrenciesAsync()
        {
            // Get raw currencies
            var currencies = await base.GetAsync<object>(
                $"{_options.Endpoint}/v1/currencies?apikey={_options.ApiKey}", false);

            // Process into response
            return GetCurrencies(JObject.FromObject(currencies));
        }

        #region Helpers

        private List<FreeCurrencyCurrencyResponse> GetCurrencies(JObject obj)
        {
            var currencies = new List<FreeCurrencyCurrencyResponse>();
            var data = obj.GetValue("data").ToObject<JObject>();
            foreach (var property in data.Properties())
            {
                var currency = data[property.Name]?.ToObject<JObject>();        
                currencies.Add(new FreeCurrencyCurrencyResponse
                { 
                    Symbol = currency["code"]?.ToString()?.ToUpper()?.Trim(), 
                    MoneySign = currency["symbol"]?.ToString() , 
                    Name = currency["name"]?.ToString() 
                });
            }

            return currencies;
        }

        private List<FreeCurrencyPriceResponse> GetPricePairs(string baseCurrency, JObject obj)
        {
            var pairs = new List<FreeCurrencyPriceResponse>();
            var data = obj.GetValue("data").ToObject<JObject>();
            foreach (var property in data.Properties())
            {
                var item = data[property.Name]?.ToString();
                if(!string.IsNullOrEmpty(item))
                {
                    decimal.TryParse(data[property.Name]?.ToString(), out decimal value);
                    pairs.Add(new FreeCurrencyPriceResponse { BaseCurrency = baseCurrency, Currency = property.Name, Price = value });
                }
            }

            return pairs;
        }

        /// <summary>
        /// Get an access token - override mandatory
        /// </summary>
        /// <returns>Access token</returns>
        protected override async Task<string> GetAccessTokenAsync()
        {
            return await Task.FromResult(_options.ApiKey);
        }

        #endregion
    }
}
