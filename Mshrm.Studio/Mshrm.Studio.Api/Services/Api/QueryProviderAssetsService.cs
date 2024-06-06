using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class QueryProviderAssetsService : IQueryProviderAssetsService
    {
        private readonly IAssetsClient _assetsClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProviderAssetsService"/> class.
        /// </summary>
        /// <param name="assetClient"></param>
        public QueryProviderAssetsService(IAssetsClient assetClient)
        {
            _assetsClient = assetClient;
        }

        /// <summary>
        /// Get all symbols supported by a provider (list of symbols)
        /// </summary>
        /// <param name="providerType">The pricing provider</param>
        /// <returns>The supported symbols for a pricing provider</returns>
        public async Task<List<string>> GetProvidersAssetSymbolsAsync(PricingProviderType providerType)
        {
            return (await _assetsClient.GetProvidersAssetSymbolsAsync(providerType))?.ToList();
        }
    }
}
