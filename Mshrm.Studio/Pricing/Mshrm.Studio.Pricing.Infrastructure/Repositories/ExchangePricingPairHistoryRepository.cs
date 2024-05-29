using Mshrm.Studio.Pricing.Api.Context;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairHistories;
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
    }
}
