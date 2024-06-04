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
        /// Get the latest prices for supported assets
        /// </summary>
        /// <param name="pricingProviderType">An optional pricing provider</param>
        /// <param name="assetType">An optional asset type</param>
        /// <param name="baseAsset">An optional base asset ie. output to what</param>
        /// <param name="symbols">Filter by supported assets</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A list of latest prices</returns>
        public async Task<List<PriceDto>> GetLatestPricesAsync(PricingProviderType? pricingProviderType, AssetType? assetType, string baseAsset, List<string>? symbols, CancellationToken cancellationToken)
        {
            return (await _priceClient.GetLatestPricesAsync(pricingProviderType, assetType, baseAsset, symbols, cancellationToken))?.ToList();
        }
    }
}
