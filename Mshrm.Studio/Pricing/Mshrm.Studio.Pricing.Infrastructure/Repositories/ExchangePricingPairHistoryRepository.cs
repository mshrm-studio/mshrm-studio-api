using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Context;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairHistories;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Pricing.Api.Repositories
{
    public class ExchangePricingPairHistoryRepository : BaseRepository<ExchangePricingPairHistory, MshrmStudioPricingDbContext>, IExchangePricingPairHistoryRepository
    {
        private readonly IExchangePricingPairHistoryFactory _exchangePricingPairHistoryFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ExchangePricingPairHistoryRepository(MshrmStudioPricingDbContext context, IExchangePricingPairHistoryFactory exchangePricingPairHistoryFactory) : base(context)
        {
            _exchangePricingPairHistoryFactory = exchangePricingPairHistoryFactory;
        }

        /// <summary>
        /// Gets all items from context - is overrideable
        /// </summary>
        /// <returns>List of items</returns>
        public override IQueryable<ExchangePricingPairHistory> GetAll(string? tableName = "ExchangePricingPairHistories")
        {
            return base.GetAll(tableName);
        }

        /// <summary>
        /// Add history for price updates
        /// </summary>
        /// <param name="exchangePricingPairId">The id of the pair being updated</param>
        /// <param name="newPrice">The new price</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="newMarketCap">The new market cap (if exists)</param>
        /// <param name="oldMarketCap">The old market cap (if exists)</param>
        /// <param name="newVolume">The new volume (if exists)</param>
        /// <param name="oldVolume">The old volume (if exists)</param>
        /// <param name="pricingProviderType">The provider updating</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The history created</returns>
        public async Task<ExchangePricingPairHistory> AddHistoryAsync(int exchangePricingPairId, decimal oldPrice, decimal newPrice,
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume, PricingProviderType pricingProviderType,
            CancellationToken cancellationToken)
        {
            var history = _exchangePricingPairHistoryFactory.CreateExchangePricingPairHistory(exchangePricingPairId, pricingProviderType, oldPrice, newPrice, oldMarketCap, newMarketCap, oldVolume, newVolume);

            Add(history);
            await SaveAsync(cancellationToken);

            return history;
        }

        /// <summary>
        /// Get the paged history prices
        /// </summary>
        /// <param name="assetId">The asset the history is for</param>
        /// <param name="pricingProviderType">The provider updating price (creating row)</param>
        /// <param name="page">The page</param>
        /// <param name="sortOrder">The sort order</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Page of price history</returns>
        public async Task<PagedResult<ExchangePricingPairHistory>> GetPagedHistory(int assetId, PricingProviderType? pricingProviderType,
             Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            var priceHistory = GetAll()
                .Include(x => x.ExchangePricingPair)
                .Where(x => x.ExchangePricingPair != null)
                .Where(x => x.ExchangePricingPair.AssetId == assetId);

            // Filter 
            if (pricingProviderType != null)
            {
                priceHistory = priceHistory.Where(x => x.PricingProviderType == pricingProviderType);
            }

            // Order 
            priceHistory = OrderSet(priceHistory, sortOrder);

            // Enumerate page
            var returnPage = await priceHistory.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<ExchangePricingPairHistory>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = priceHistory.Count()
            };
        }

        #region Helpers

        /// <summary>
        /// Orders set in an enumerable list
        /// </summary>
        /// <param name="set">The list to order</param>
        /// <param name="sortOrder">The sort order details</param>
        /// <returns>Sorted list</returns>
        private IQueryable<ExchangePricingPairHistory> OrderSet(IQueryable<ExchangePricingPairHistory> set, SortOrder sortOrder)
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
