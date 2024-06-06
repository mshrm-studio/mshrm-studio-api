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
        /// Get all assets supported by a provider
        /// </summary>
        /// <param name="providerType">The pricing provider</param>
        /// <returns>The supported assets for a pricing provider</returns>
        public async Task<List<ProviderAssetDto>> GetProvidersAssetsAsync(PricingProviderType providerType)
        {
            return (await _assetsClient.GetProvidersAssetsAsync(providerType))?.ToList();
        }
    }
}
