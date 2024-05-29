using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.HttpService.MetalsDev;
using Mshrm.Studio.Pricing.Api.Models.HttpService.PolygonIO;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Services.Http.Bases;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Mshrm.Studio.Pricing.Api.Services.Http
{
    public class MetalsDevService : BaseHttpService, IMetalsDevService
    {
        private readonly MetalsDevServiceOptions _options;
        private readonly ILogger<MetalsDevService> _logger;

        public MetalsDevService(HttpClient client, IOptions<MetalsDevServiceOptions> options, ILogger<MetalsDevService> logger) : base(client)
        {
            _options = options.Value;
            _logger = logger;
        }
        
        /// <summary>
        /// Prices for 1 of a base currency
        /// </summary>
        /// <param name="unit">The unit to get price of</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <returns>Prices for 1 of a base currency</returns>
        public async Task<MetalsDevPriceResponse> GetPricesAsync(string baseCurrency = "USD", string unit = "toz")
        {
            var url = $"{_options.Endpoint}/v1/latest?api_key={_options.ApiKey}&currency={baseCurrency}&unit={unit}";
            
            // Get raw prices
            var prices = await base.GetAsync<object>(url, false);

            return FormatPriceResponse(baseCurrency, JObject.FromObject(prices));
        }

        /// <summary>
        /// Get all assets
        /// </summary>
        /// <returns>Assets</returns>
        public async Task<List<MetalsDevAsset>> GetAssestAsync()
        {
            var allPrices = await GetPricesAsync();

            return allPrices.PricePairs.Select(x => new MetalsDevAsset()
            {
                Name = x.Symbol.UppercaseFirstLetter(),
                Symbol = x.Symbol.ToUpper(),
            }).ToList();
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

        private MetalsDevPriceResponse FormatPriceResponse(string baseCurrency, JObject obj)
        {
            var pairs = new MetalsDevPriceResponse();
            var data = obj.GetValue("metals").ToObject<JObject>();
            foreach (var property in data.Properties())
            {
                var value = data[property.Name]?.ToString();
                if (value != null)
                {
                    decimal.TryParse(value, out decimal priceValue);
                    pairs.PricePairs.Add(new MetalsDevPricePair
                    { 
                        Symbol = property.Name, 
                        Price = priceValue
                    });
                }
            }

            return pairs;
        }

        #endregion
    }
}
