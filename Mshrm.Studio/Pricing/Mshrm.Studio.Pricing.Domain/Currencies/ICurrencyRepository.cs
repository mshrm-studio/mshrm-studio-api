using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Pricing.Api.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// Create a new currency
        /// </summary>
        /// <param name="name">The display name</param>
        /// <param name="description">A short description</param>
        /// <param name="providerType">The provider</param>
        /// <param name="currencyType">The type of currency</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="symbolNative">The native symbol ie. $</param>
        /// <param name="logoGuidId">The logos guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new currency</returns>
        public Task<Currency> CreateCurrencyAsync(string name, string? description, PricingProviderType providerType,
            CurrencyType currencyType, string symbol, string symbolNative, Guid? logoGuidId, CancellationToken cancellationToken);

        /// <summary>
        /// Get a page of currencies
        /// </summary>
        /// <param name="search">A search term</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="name">The name</param>
        /// <param name="pricingProviderType">The provider used to import data</param>
        /// <param name="currencyType">The type of currency</param>
        /// <param name="page">The page</param>
        /// <param name="sortOrder">The sort order</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of currencies</returns>
        public Task<PagedResult<Currency>> GetCurrenciesPagedAsync(string? search, string? symbol, string? name, PricingProviderType? pricingProviderType,
            CurrencyType? currencyType, Page page, SortOrder sortOrder, CancellationToken cancellationToken);

        /// <summary>
        /// Get a list of currencies
        /// </summary>
        /// <param name="type">The currency type</param>
        /// <param name="providerType">The type of provider used</param>
        /// <param name="active">If the currency is active or not</param>
        /// <param name="symbols">Filter by symbols</param>
        /// <returns>A list of filtered currencies</returns>
        public Task<List<Currency>> GetCurrenciesReadOnlyAsync(CurrencyType? type, PricingProviderType? providerType, bool? active, List<string>? symbols);

        /// <summary>
        /// Get a currency by symbol/type
        /// </summary>
        /// <param name="symbol">The symbol (case insensitive)</param>
        /// <param name="currencyType">The type</param>
        /// <param name="active">If the currency is active</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A currency if found</returns>
        public Task<Currency?> GetCurrencyAsync(string symbol, CurrencyType currencyType, bool? active, CancellationToken cancellationToken);

        /// <summary>
        /// Get a currency by guid id
        /// </summary>
        /// <param name="guidId">The id</param>
        /// <param name="active">If the currency is active</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A currency if found</returns>
        public Task<Currency?> GetCurrencyAsync(Guid guidId, bool? active, CancellationToken cancellationToken);

        /// <summary>
        /// Update a currency (all except symbol)
        /// </summary>
        /// <param name="currencyId">The currency to update</param>
        /// <param name="name">A name</param>
        /// <param name="description">A description</param>
        /// <param name="symbolNative">The symbol used for what its measured in</param>
        /// <param name="providerType">The provider to import price from</param>
        /// <param name="currencyType">The type of currency</param>
        /// <param name="logoGuidId">A logo</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated currency</returns>
        public Task<Currency?> UpdateCurrencyAsync(Guid currencyId, string name, string? description, PricingProviderType providerType, CurrencyType currencyType, string symbolNative,
            Guid? logoGuidId, CancellationToken cancellationToken);
    }
}
