using Mshrm.Studio.Api.Clients.Pricing;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryPriceHistoryService
    {
        /// <summary>
        /// Gets price history
        /// </summary>
        /// <param name="pricingProviderType">The provider used to import</param>
        /// <param name="baseAssetGuidId">The base asset</param>
        /// <param name="assetGuidId">Asset to get history for</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Price history</returns>
        Task<PageResultDtoOfAssetPriceHistoryDto> GetPagedPriceHistoryAsync(string assetGuidId, string baseAssetGuidId, PricingProviderType? pricingProviderType, string orderProperty, 
            Order order, uint pageNumber, uint perPage, CancellationToken cancellationToken);
    }
}
