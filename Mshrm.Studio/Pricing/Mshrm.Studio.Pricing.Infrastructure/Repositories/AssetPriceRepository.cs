using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Context;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Pricing.Api.Repositories
{
    public class AssetPriceRepository : BaseRepository<AssetPrice, MshrmStudioPricingDbContext>, IAssetPriceRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public AssetPriceRepository(MshrmStudioPricingDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get the latest asset prices
        /// </summary>
        /// <param name="assetIds">The assets to get - all if left null/empty</param>
        /// <param name="pricingProviderType">The provider used to generate price</param>
        /// <param name="assetType">The asset type the price is for</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Asset prices for a set of symbols</returns>
        public async Task<List<AssetPrice>> GetLatestAssetPricesReadOnlyAsync(List<int>? assetIds, PricingProviderType? pricingProviderType,
            AssetType? assetType, CancellationToken cancellationToken)
        {
            var assetPrices = GetAll()
                .AsNoTracking()
                .Include(x => x.Asset)
                .Include(x => x.BaseAsset)
                .Where(x => true);

            if ((assetIds?.Any() ?? false))
            {
                assetPrices = assetPrices.Where(x => assetIds.Contains(x.AssetId));
            }

            if (pricingProviderType.HasValue)
            {
                assetPrices = assetPrices.Where(x => x.Asset.ProviderType == pricingProviderType);
            }

            if (assetType.HasValue)
            {
                assetPrices = assetPrices.Where(x => x.Asset.AssetType == assetType);
            }

            return await assetPrices.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Add a set of asset prices
        /// </summary>
        /// <param name="allAssetPricesToAdd">All asset prices to add</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The added asset prices</returns>
        public async Task<List<AssetPrice>> CreateAssetPricesAsync(List<AssetPrice> allAssetPricesToAdd, CancellationToken cancellationToken)
        {
            _context.AssetPrices.AddRange(allAssetPricesToAdd);
            await SaveAsync(cancellationToken);

            return allAssetPricesToAdd;
        }

        /// <summary>
        /// Update a set of existing asset prices
        /// </summary>
        /// <param name="allAssetPricesToUpdate">The asset prices to update</param>
        /// <param name="pricingProviderType">The provider used in update</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The updated asset prices</returns>
        public async Task<List<AssetPrice>> UpdateAssetPricesAsync(List<AssetPrice> allAssetPricesToUpdate, PricingProviderType pricingProviderType,
            CancellationToken cancellationToken)
        {
            var allIds = allAssetPricesToUpdate.Select(x => x.Id).ToList();
            var allExisting = GetAll().Where(x => allIds.Contains(x.Id)).ToList();

            foreach (var assetPriceToUpdate in allAssetPricesToUpdate)
            {
                var existingAssetPrice = allExisting.FirstOrDefault(x => x.Id == assetPriceToUpdate.Id);
                existingAssetPrice.UpdateData(assetPriceToUpdate.Price, assetPriceToUpdate.MarketCap, assetPriceToUpdate.Volume, pricingProviderType);
                Update(existingAssetPrice);
            }

            await SaveAsync(cancellationToken);

            return allExisting;
        }
    }
}
