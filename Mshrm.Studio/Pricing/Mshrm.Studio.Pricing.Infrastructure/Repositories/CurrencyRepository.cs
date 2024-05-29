using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Context;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Domain.Currencies;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Pricing.Api.Repositories
{
    /// <summary>
    /// Currency repository
    /// </summary>
    public class CurrencyRepository : BaseRepository<Currency, MshrmStudioPricingDbContext>, ICurrencyRepository
    {
        private readonly ICurrencyFactory _currencyFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public CurrencyRepository(MshrmStudioPricingDbContext context, ICurrencyFactory currencyFactory) : base(context)
        {
            _currencyFactory = currencyFactory;
        }

        /// <summary>
        /// Gets all items from context - is overrideable
        /// </summary>
        /// <returns>List of items</returns>
        public override IQueryable<Currency> GetAll(string? tableName = "Currencies")
        {
            return base.GetAll(tableName);
        }

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
        public async Task<Currency> CreateCurrencyAsync(string name, string? description, PricingProviderType providerType,
            CurrencyType currencyType, string symbol, string symbolNative, Guid? logoGuidId, CancellationToken cancellationToken)
        {
            var newCurrency = _currencyFactory.CreateCurrency(providerType, currencyType, name, symbol, symbolNative, description, logoGuidId);

            Add(newCurrency);
            await SaveAsync(cancellationToken);

            return newCurrency;
        }

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
        public async Task<PagedResult<Currency>> GetCurrenciesPagedAsync(string? search, string? symbol, string? name, PricingProviderType? pricingProviderType,
            CurrencyType? currencyType, Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            var currencies = GetAll()
                .AsNoTracking();

            // Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                currencies = currencies.Where(x => x.Name.ToLower().Contains(search.ToLower().Trim()) ||
                    x.Symbol.ToLower().Contains(search.ToLower().Trim()));
            }

            // Filter by symbol
            if (!string.IsNullOrEmpty(symbol))
            {
                currencies = currencies.Where(x => x.Symbol.ToLower().Contains(symbol.ToLower().Trim()));
            }

            // Filter by name
            if (!string.IsNullOrEmpty(name))
            {
                currencies = currencies.Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));
            }

            // Filter by pricing provider
            if (pricingProviderType != null)
            {
                currencies = currencies.Where(x => x.ProviderType == pricingProviderType);
            }

            // Filter by currency type
            if (currencyType != null)
            {
                currencies = currencies.Where(x => x.CurrencyType == currencyType);
            }

            // Order 
            currencies = OrderSet(currencies, sortOrder);

            // Enumerate page
            var returnPage = await currencies.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<Currency>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = currencies.Count()
            };
        }

        /// <summary>
        /// Get a list of currencies
        /// </summary>
        /// <param name="type">The currency type</param>
        /// <param name="providerType">The type of provider used</param>
        /// <param name="active">If the currency is active or not</param>
        /// <param name="symbols">Filter by symbols</param>
        /// <returns>A list of filtered currencies</returns>
        public async Task<List<Currency>> GetCurrenciesReadOnlyAsync(CurrencyType? type, PricingProviderType? providerType, bool? active, List<string>? symbols)
        {
            var currencies = GetAll().AsNoTracking();

            if (type != null)
            {
                currencies = currencies.Where(x => x.CurrencyType == type);
            }

            if (providerType != null)
            {
                currencies = currencies.Where(x => x.ProviderType == providerType);
            }

            if (active != null)
            {
                currencies = currencies.Where(x => x.Active == active);
            }

            if ((symbols?.Any() ?? false))
            {
                symbols = symbols.Select(x => x.Trim().ToUpper()).ToList();
                currencies = currencies.Where(x => symbols.Contains(x.Symbol));
            }

            return await currencies.ToListAsync();
        }

        /// <summary>
        /// Get a currency by symbol/type
        /// </summary>
        /// <param name="symbol">The symbol (case insensitive)</param>
        /// <param name="currencyType">The type</param>
        /// <param name="active">If the currency is active</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A currency if found</returns>
        public async Task<Currency?> GetCurrencyAsync(string symbol, CurrencyType currencyType, bool? active, CancellationToken cancellationToken)
        {
            var currencies = GetAll()
                .Where(x => x.Symbol == symbol.ToUpper().Trim())
                .Where(x => x.CurrencyType == currencyType);

            if (active.HasValue)
            {
                currencies = currencies.Where(x => x.Active == active);
            }

            return await currencies.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get a currency by guid id
        /// </summary>
        /// <param name="guidId">The id</param>
        /// <param name="active">If the currency is active</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A currency if found</returns>
        public async Task<Currency?> GetCurrencyAsync(Guid guidId, bool? active, CancellationToken cancellationToken)
        {
            var currencies = GetAll()
                .Where(x => x.GuidId == guidId);

            if (active.HasValue)
            {
                currencies = currencies.Where(x => x.Active == active);
            }

            return await currencies.FirstOrDefaultAsync(cancellationToken);
        }

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
        public async Task<Currency?> UpdateCurrencyAsync(Guid currencyId, string name, string? description, PricingProviderType providerType, CurrencyType currencyType, string symbolNative,
            Guid? logoGuidId, CancellationToken cancellationToken)
        {
            var existing = GetAll().FirstOrDefault(x => x.GuidId == currencyId);
            if (existing != null)
            {
                existing.SetName(name);
                existing.SetDescription(description);
                existing.SetProviderType(providerType);
                existing.SetCurrencyType(currencyType);
                existing.SetSymbolNative(symbolNative);

                if (logoGuidId.HasValue)
                    existing.SetLogo(logoGuidId);

                Update(existing);
                await SaveAsync(cancellationToken);

                return existing;
            }

            return null;
        }

        #region Helpers

        /// <summary>
        /// Orders set in an enumerable list
        /// </summary>
        /// <param name="set">The list to order</param>
        /// <param name="sortOrder">The sort order details</param>
        /// <returns>Sorted list</returns>
        private IQueryable<Currency> OrderSet(IQueryable<Currency> set, SortOrder sortOrder)
        {
            return (sortOrder.PropertyName.Trim(), sortOrder.Order) switch
            {
                ("createdDate", Order.Ascending) => set.OrderBy(x => x.CreatedDate),
                ("createdDate", Order.Descending) => set.OrderByDescending(x => x.CreatedDate),
                _ => sortOrder.Order == Order.Descending ? set.OrderBy(x => x.CreatedDate) : set.OrderByDescending(x => x.CreatedDate)
            };
        }

        #endregion
    }
}
