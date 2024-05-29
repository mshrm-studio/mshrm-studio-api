using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula;
using Mshrm.Studio.Pricing.Api.Models.HttpService.PolygonIO;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Services.Http.Bases;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Http
{
    public class PolygonIOService : BaseHttpService, IPolygonIOService
    {
        private readonly PolygonIOServiceOptions _options;
        private readonly ILogger<PolygonIOService> _logger;

        public PolygonIOService(HttpClient client, IOptions<PolygonIOServiceOptions> options, ILogger<PolygonIOService> logger) : base(client)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="currencies">The currencies to get</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public async Task<List<PolygonIOPriceResponse>> GetPricesAsync(List<string> currencies, string baseCurrency = "USD")
        {
            var priceResponses = new List<PolygonIOPriceResponse>();

            // Polygon API has rate limit so this needs to be split across a minute - we set to 2 mins so we can have some calls free if we need to update currency
            var waitTimeInMillis = Convert.ToInt32((120m / _options.RequestsPerMinute) * 1000m);

            foreach (var currency in currencies)
            {
                var priceResponse = await GetPriceAsync(currency);
                if (priceResponse != null)
                    priceResponses.Add(priceResponse);

                // We need to wait
                await Task.Delay(waitTimeInMillis);
            }

            return priceResponses;
        }

        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="symbol">The currency symbol to get</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public async Task<PolygonIOPriceResponse> GetPriceAsync(string symbol)
        {
            var url = $"{_options.Endpoint}/v1/open-close/{symbol}/{DateTime.UtcNow.Date.AddDays(-1).ToString("yyyy-MM-dd")}?adjusted=true&apiKey={_options.ApiKey}";

            // Get raw prices
            return await base.GetAsync<PolygonIOPriceResponse>(url, false);
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currencies</returns>
        public async Task<PolygonIOCurrencyResponse> GetCurrenciesAsync(string? symbol)
        {
            var url = (string.IsNullOrEmpty(symbol)) ? $"{_options.Endpoint}/v3/reference/tickers?active=true&apiKey=ZBLk8ibFBjEZGIB9i38RZ4n2Oha9YUZJ&apiKey={_options.ApiKey}" :
                $"{_options.Endpoint}/v3/reference/tickers?active=true&apiKey={_options.ApiKey}&ticker={symbol}";
            // Get raw currencies
            return await base.GetAsync<PolygonIOCurrencyResponse>(url, false);
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

        #endregion
    }
}
