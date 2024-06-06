using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Context;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Pricing.Api.Repositories
{
    public class ExchangePricingPairRepository : BaseRepository<ExchangePricingPair, MshrmStudioPricingDbContext>, IExchangePricingPairRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ExchangePricingPairRepository(MshrmStudioPricingDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get the latest exchange pricing pairs
        /// </summary>
        /// <param name="assetIds">The assets to get - all if left null/empty</param>
        /// <param name="pricingProviderType">The provider used to generate price</param>
        /// <param name="assetType">The asset type the price is for</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Pricing pairs for a set of symbols</returns>
        public async Task<List<ExchangePricingPair>> GetLatestExchangePricingPairsReadOnlyAsync(List<int>? assetIds, PricingProviderType? pricingProviderType,
            AssetType? assetType, CancellationToken cancellationToken)
        {
            var pairs = GetAll()
                .AsNoTracking()
                .Include(x => x.Asset)
                .Include(x => x.BaseAsset)
                .Where(x => true);

            if ((assetIds?.Any() ?? false))
            {
                pairs = pairs.Where(x => assetIds.Contains(x.AssetId));
            }

            if (pricingProviderType.HasValue)
            {
                pairs = pairs.Where(x => x.Asset.ProviderType == pricingProviderType);
            }

            if (assetType.HasValue)
            {
                pairs = pairs.Where(x => x.Asset.AssetType == assetType);
            }

            return await pairs.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Add a set of prices
        /// </summary>
        /// <param name="allPairsToAdd">All prices to add</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The added pairs</returns>
        public async Task<List<ExchangePricingPair>> CreateExchangePricingPairsAsync(List<ExchangePricingPair> allPairsToAdd, CancellationToken cancellationToken)
        {
            _context.ExchangePricingPairs.AddRange(allPairsToAdd);
            await SaveAsync(cancellationToken);

            return allPairsToAdd;
        }

        /// <summary>
        /// Update a set of existing pairs
        /// </summary>
        /// <param name="allPairsToUpdate">The pairs to update</param>
        /// <param name="pricingProviderType">The provider used in update</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The updated pairs</returns>
        public async Task<List<ExchangePricingPair>> UpdateExchangePricingPairsAsync(List<ExchangePricingPair> allPairsToUpdate, PricingProviderType pricingProviderType,
            CancellationToken cancellationToken)
        {
            var allIds = allPairsToUpdate.Select(x => x.Id).ToList();
            var allExisting = GetAll().Where(x => allIds.Contains(x.Id)).ToList();

            foreach (var pairToUpdate in allPairsToUpdate)
            {
                var existingPair = allExisting.FirstOrDefault(x => x.Id == pairToUpdate.Id);
                existingPair.UpdateData(pairToUpdate.Price, pairToUpdate.MarketCap, pairToUpdate.Volume, pricingProviderType);
                Update(existingPair);
            }

            await SaveAsync(cancellationToken);

            return allExisting;
        }
    }
}
