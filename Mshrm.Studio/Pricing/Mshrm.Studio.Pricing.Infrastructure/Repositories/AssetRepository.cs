using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Context;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Domain.Assets;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Pricing.Api.Repositories
{
    /// <summary>
    /// Asset repository
    /// </summary>
    public class AssetRepository : BaseRepository<Asset, MshrmStudioPricingDbContext>, IAssetRepository
    {
        private readonly IAssetFactory _assetFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public AssetRepository(MshrmStudioPricingDbContext context, IAssetFactory assetFactory) : base(context)
        {
            _assetFactory = assetFactory;
        }

        /// <summary>
        /// Gets all items from context - is overrideable
        /// </summary>
        /// <returns>List of items</returns>
        public override IQueryable<Asset> GetAll(string? tableName = "Assets")
        {
            return base.GetAll(tableName);
        }

        /// <summary>
        /// Create a new asset
        /// </summary>
        /// <param name="name">The display name</param>
        /// <param name="description">A short description</param>
        /// <param name="providerType">The provider</param>
        /// <param name="assetType">The type of asset</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="symbolNative">The native symbol ie. $</param>
        /// <param name="logoGuidId">The logos guid id</param>
        /// <param name="decimalPlaces">The number of decimal places to display in</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new asset</returns>
        public async Task<Asset> CreateAssetAsync(string name, string? description, PricingProviderType providerType, AssetType assetType, string symbol, string symbolNative, 
            Guid? logoGuidId, int decimalPlaces, CancellationToken cancellationToken)
        {
            var newAsset = _assetFactory.CreateAsset(providerType, assetType, name, symbol, symbolNative, description, logoGuidId, decimalPlaces);

            Add(newAsset);
            await SaveAsync(cancellationToken);

            return newAsset;
        }

        /// <summary>
        /// Get a page of assets
        /// </summary>
        /// <param name="search">A search term</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="name">The name</param>
        /// <param name="pricingProviderType">The provider used to import data</param>
        /// <param name="assetType">The type of asset</param>
        /// <param name="page">The page</param>
        /// <param name="sortOrder">The sort order</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of assets</returns>
        public async Task<PagedResult<Asset>> GetAssetsPagedAsync(string? search, string? symbol, string? name, PricingProviderType? pricingProviderType,
            AssetType? assetType, Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            var assets = GetAll()
                .AsNoTracking();

            // Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                assets = assets.Where(x => x.Name.ToLower().Contains(search.ToLower().Trim()) ||
                    x.Symbol.ToLower().Contains(search.ToLower().Trim()));
            }

            // Filter by symbol
            if (!string.IsNullOrEmpty(symbol))
            {
                assets = assets.Where(x => x.Symbol.ToLower().Contains(symbol.ToLower().Trim()));
            }

            // Filter by name
            if (!string.IsNullOrEmpty(name))
            {
                assets = assets.Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));
            }

            // Filter by pricing provider
            if (pricingProviderType != null)
            {
                assets = assets.Where(x => x.ProviderType == pricingProviderType);
            }

            // Filter by asset type
            if (assetType != null)
            {
                assets = assets.Where(x => x.AssetType == assetType);
            }

            // Order 
            assets = OrderSet(assets, sortOrder);

            // Enumerate page
            var returnPage = await assets.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<Asset>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = assets.Count()
            };
        }

        /// <summary>
        /// Get a list of assets
        /// </summary>
        /// <param name="assetType">The asset type</param>
        /// <param name="providerType">The type of provider used</param>
        /// <param name="active">If the asset is active or not</param>
        /// <param name="symbols">Filter by symbols</param>
        /// <returns>A list of filtered assets</returns>
        public async Task<List<Asset>> GetAssetsReadOnlyAsync(AssetType? type, PricingProviderType? providerType, bool? active, List<string>? symbols)
        {
            var assets = GetAll().AsNoTracking();

            if (type != null)
            {
                assets = assets.Where(x => x.AssetType == type);
            }

            if (providerType != null)
            {
                assets = assets.Where(x => x.ProviderType == providerType);
            }

            if (active != null)
            {
                assets = assets.Where(x => x.Active == active);
            }

            if ((symbols?.Any() ?? false))
            {
                symbols = symbols.Select(x => x.Trim().ToUpper()).ToList();
                assets = assets.Where(x => symbols.Contains(x.Symbol));
            }

            return await assets.ToListAsync();
        }

        /// <summary>
        /// Get a asset by symbol/type
        /// </summary>
        /// <param name="symbol">The symbol (case insensitive)</param>
        /// <param name="assetType">The type</param>
        /// <param name="active">If the asset is active</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A asset if found</returns>
        public async Task<Asset?> GetAssetAsync(string symbol, AssetType assetType, bool? active, CancellationToken cancellationToken)
        {
            var assets = GetAll()
                .Where(x => x.Symbol == symbol.ToUpper().Trim())
                .Where(x => x.AssetType == assetType);

            if (active.HasValue)
            {
                assets = assets.Where(x => x.Active == active);
            }

            return await assets.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get a asset by guid id
        /// </summary>
        /// <param name="guidId">The id</param>
        /// <param name="active">If the asset is active</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A asset if found</returns>
        public async Task<Asset?> GetAssetAsync(Guid guidId, bool? active, CancellationToken cancellationToken)
        {
            var assets= GetAll()
                .Where(x => x.GuidId == guidId);

            if (active.HasValue)
            {
                assets = assets.Where(x => x.Active == active);
            }

            return await assets.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Update a asset (all except symbol)
        /// </summary>
        /// <param name="assetId">The asset to update</param>
        /// <param name="name">A name</param>
        /// <param name="description">A description</param>
        /// <param name="symbolNative">The symbol used for what its measured in</param>
        /// <param name="providerType">The provider to import price from</param>
        /// <param name="assetType">The type of asset</param>
        /// <param name="logoGuidId">A logo</param>
        /// <param name="decimalPlaces">The number of decimal places to display in</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated asset</returns>
        public async Task<Asset?> UpdateAssetAsync(Guid assetId, string name, string? description, PricingProviderType providerType, AssetType assetType, string symbolNative,
            Guid? logoGuidId, int decimalPlaces, CancellationToken cancellationToken)
        {
            var existing = GetAll().FirstOrDefault(x => x.GuidId == assetId);
            if (existing != null)
            {
                existing.SetName(name);
                existing.SetDescription(description);
                existing.SetProviderType(providerType);
                existing.SetAssetType(assetType);
                existing.SetSymbolNative(symbolNative);
                existing.SetDecimalPlaces(decimalPlaces);

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
        private IQueryable<Asset> OrderSet(IQueryable<Asset> set, SortOrder sortOrder)
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
