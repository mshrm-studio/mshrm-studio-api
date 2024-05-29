using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Currencies;
using Order = Mshrm.Studio.Shared.Enums.Order;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryCurrenciesService
    {
        /// <summary>
        /// Get currencies paged
        /// </summary>
        /// <param name="search">A search term</param>
        /// <param name="symbol">A currency symbol</param>
        /// <param name="name">The currencies name</param>
        /// <param name="pricingProviderType">The provider type for where to import currency from</param>
        /// <param name="currencyType">The type of currency</param>
        /// <param name="orderProperty">The property to order result set by</param>
        /// <param name="order">The order in which to return the result set</param>
        /// <param name="pageNumber">The page to return</param>
        /// <param name="perPage">The number of results per page</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A paged of currency responses</returns>
        Task<PageResultDtoOfCurrencyDto> GetCurrenciesAsync(string? search, string? symbol, string? name, PricingProviderType? pricingProviderType, CurrencyType? currencyType, string orderProperty, Order order, 
            uint pageNumber, uint perPage, CancellationToken cancellationToken);

        /// <summary>
        /// Get a currency by guid
        /// </summary>
        /// <param name="guid">The guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A currency</returns>
        Task<CurrencyDto> GetCurrencyByGuidAsync(Guid guid, CancellationToken cancellationToken);
    }
}
