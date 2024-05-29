using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Services.Http.Bases;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Http
{
    public class MobulaService : BaseHttpService, IMobulaService
    {
        private readonly MobulaServiceOptions _options;
        private readonly ILogger<MobulaService> _logger;

        public MobulaService(HttpClient client, IOptions<MobulaServiceOptions> options, ILogger<MobulaService> logger) : base(client)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="currencies">The currencies to get prices of</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public async Task<MobulaPriceResponse> GetPricesAsync(List<string> currencies, string baseCurrency = "USD")
        {
            var formattedCurrencies = string.Join(',', currencies.Select(x => x.Trim().ToUpper()));

            // Get raw prices
            var prices = await base.GetAsync<object>(
                $"{_options.Endpoint}/api/1/market/multi-data?symbols={formattedCurrencies}", true);

            // Process into response
            return FormatPriceResponse(baseCurrency, JObject.FromObject(prices));
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public async Task<MobulaCurrencyResponse> GetCurrenciesAsync(string? symbol)
        {
            var url = (string.IsNullOrEmpty(symbol)) ? $"{_options.Endpoint}/api/1/all" : $"{_options.Endpoint}/api/1/all?symbol={symbol}";
            // Get raw currencies
            return await base.GetAsync<MobulaCurrencyResponse>(url, false);
        }

        #region Helpers

        /// <summary>
        /// Get an access token - override mandatory
        /// </summary>
        /// <returns>Access token</returns>
        protected override async Task<string> GetAccessTokenAsync()
        {
            return await Task.FromResult(_options.ApiKey);
        }

        private MobulaPriceResponse FormatPriceResponse(string baseCurrency, JObject obj)
        {
            var pairs = new MobulaPriceResponse();
            var data = obj.GetValue("data").ToObject<JObject>();
            foreach (var property in data.Properties())
            {
                var pair = data[property.Name]?.ToObject<JObject>();
                if (pair != null)
                {
                    decimal.TryParse(pair["price"]?.ToString(), out decimal priceValue);
                    decimal.TryParse(pair["volume"]?.ToString(), out decimal volumeValue);
                    decimal.TryParse(pair["market_cap"]?.ToString(), out decimal marketCapValue);
                    pairs.PricePairs.Add(new MobulaPricePair 
                    { 
                        Name = pair["name"].ToString(), 
                        Symbol = pair["symbol"].ToString(), 
                        Volume = volumeValue,
                        MarketCap = marketCapValue,
                        Price = priceValue
                    });
                }
            }

            return pairs;
        }

        #endregion
    }
}
