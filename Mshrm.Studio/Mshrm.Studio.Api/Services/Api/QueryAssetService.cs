using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Assets;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Order = Mshrm.Studio.Shared.Enums.Order;

namespace Mshrm.Studio.Api.Services.Api
{
    public class QueryAssetService : IQueryAssetService
    {
        private readonly IAssetsClient _assetsClient;

        public QueryAssetService(IAssetsClient assetsClient)
        {
            _assetsClient = assetsClient;
        }

        /// <summary>
        /// Get assets paged
        /// </summary>
        /// <param name="search">A search term</param>
        /// <param name="symbol">A asset symbol</param>
        /// <param name="name">The assets name</param>
        /// <param name="pricingProviderType">The provider type for where to import asset from</param>
        /// <param name="assetType">The type of asset</param>
        /// <param name="orderProperty">The property to order result set by</param>
        /// <param name="order">The order in which to return the result set</param>
        /// <param name="pageNumber">The page to return</param>
        /// <param name="perPage">The number of results per page</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A paged of asset responses</returns>
        public async Task<PageResultDtoOfAssetDto> GetAssetsAsync(string? search, string? symbol, string? name, PricingProviderType? pricingProviderType, AssetType? assetType, string orderProperty, Order order,
            uint pageNumber, uint perPage, CancellationToken cancellationToken)
        {
            return await _assetsClient.GetSupportedAssetsAsync(search, symbol, name, pricingProviderType, assetType, orderProperty, (Clients.Pricing.Order)order, (int)pageNumber, (int)perPage);
        }

        /// <summary>
        /// Get a asset by guid
        /// </summary>
        /// <param name="guid">The guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A asset</returns>
        public async Task<AssetDto> GetAssetByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            //return await _assetClient.GetAssetByGuidAsync();
            //TODO:
            return null;
        }
    }
}
