using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Currencies;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Order = Mshrm.Studio.Shared.Enums.Order;

namespace Mshrm.Studio.Api.Services.Api
{
    public class QueryCurrenciesService : IQueryCurrenciesService
    {
        private readonly ICurrenciesClient _currenciesClient;

        public QueryCurrenciesService(ICurrenciesClient currenciesClient)
        {
            _currenciesClient = currenciesClient;
        }

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
        public async Task<PageResultDtoOfCurrencyDto> GetCurrenciesAsync(string? search, string? symbol, string? name, PricingProviderType? pricingProviderType, CurrencyType? currencyType, string orderProperty, Order order,
            uint pageNumber, uint perPage, CancellationToken cancellationToken)
        {
            return await _currenciesClient.GetSupportedCurrenciesAsync(search, symbol, name, pricingProviderType, currencyType, orderProperty, (Clients.Pricing.Order)order, (int)pageNumber, (int)perPage);
        }

        /// <summary>
        /// Get a currency by guid
        /// </summary>
        /// <param name="guid">The guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A currency</returns>
        public async Task<CurrencyDto> GetCurrencyByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            //return await _currenciesClient.GetCurrencyByGuidAsync();
            //TODO:
            return null;
        }
    }
}
