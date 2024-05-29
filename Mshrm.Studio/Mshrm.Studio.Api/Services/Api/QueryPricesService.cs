using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class QueryPricesService : IQueryPricesService
    {
        private readonly IPricesClient _priceClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="priceClient">Pricing client</param>
        public QueryPricesService(IPricesClient priceClient)
        {
            _priceClient = priceClient;
        }

        /// <summary>
        /// Get the latest prices for supported currencies
        /// </summary>
        /// <param name="pricingProviderType">An optional pricing provider</param>
        /// <param name="currencyType">An optional currency type</param>
        /// <param name="baseCurrency">An optional base currencie ie. output to what</param>
        /// <param name="symbols">Filter by supported currencies</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A list of latest prices</returns>
        public async Task<List<PriceDto>> GetLatestPricesAsync(PricingProviderType? pricingProviderType, CurrencyType? currencyType, string baseCurrency, List<string>? symbols, CancellationToken cancellationToken)
        {
            return (await _priceClient.GetLatestPricesAsync(pricingProviderType, currencyType, baseCurrency, symbols, cancellationToken))?.ToList();
        }
    }
}
