using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Models.Provider;
using Mshrm.Studio.Pricing.Api.Services.Http.Bases;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;

namespace Mshrm.Studio.Pricing.Api.Services.Providers
{
    public class TwelveDataService : BaseHttpService, ITwelveDataService
    {
        private readonly TwelveDataServiceOptions _twelveDataProviderOptions;
        private readonly ILogger<TwelveDataService> _logger;

        public TwelveDataService(HttpClient client, IOptions<TwelveDataServiceOptions> twelveDataProviderOptions, ILogger<TwelveDataService> logger) : base(client) 
        {
            _twelveDataProviderOptions = twelveDataProviderOptions.Value;
            _logger = logger;
        }

        /// <summary>
        /// Prices for 1 of a base asset
        /// </summary>
        /// <param name="baseAsset">The base asset</param>
        /// <returns>Prices for 1 of a base asset</returns>
        public async Task<List<PricePair>> GetPricesAsync(string baseAsset = "USD")
        {
            return await base.GetAsync<List<PricePair>>($"{_twelveDataProviderOptions.Endpoint}/", false);
        }
    }
}
