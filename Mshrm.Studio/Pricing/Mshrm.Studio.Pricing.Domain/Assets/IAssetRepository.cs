using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Pricing.Api.Repositories.Interfaces
{
    public interface IAssetRepository
    {
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
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new asset</returns>
        public Task<Asset> CreateAssetAsync(string name, string? description, PricingProviderType providerType,
            AssetType assetType, string symbol, string symbolNative, Guid? logoGuidId, CancellationToken cancellationToken);

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
        public Task<PagedResult<Asset>> GetAssetsPagedAsync(string? search, string? symbol, string? name, PricingProviderType? pricingProviderType,
            AssetType? assetType, Page page, SortOrder sortOrder, CancellationToken cancellationToken);

        /// <summary>
        /// Get a list of assets
        /// </summary>
        /// <param name="assetType">The asset type</param>
        /// <param name="providerType">The type of provider used</param>
        /// <param name="active">If the asset is active or not</param>
        /// <param name="symbols">Filter by symbols</param>
        /// <returns>A list of filtered assets</returns>
        public Task<List<Asset>> GetAssetsReadOnlyAsync(AssetType? assetType, PricingProviderType? providerType, bool? active, List<string>? symbols);

        /// <summary>
        /// Get a asset by symbol/type
        /// </summary>
        /// <param name="symbol">The symbol (case insensitive)</param>
        /// <param name="assetType">The type</param>
        /// <param name="active">If the asset is active</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A asset if found</returns>
        public Task<Asset?> GetAssetAsync(string symbol, AssetType assetType, bool? active, CancellationToken cancellationToken);

        /// <summary>
        /// Get a asset by guid id
        /// </summary>
        /// <param name="guidId">The id</param>
        /// <param name="active">If the asset is active</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A asset if found</returns>
        public Task<Asset?> GetAssetAsync(Guid guidId, bool? active, CancellationToken cancellationToken);

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
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated asset</returns>
        public Task<Asset?> UpdateAssetAsync(Guid assetId, string name, string? description, PricingProviderType providerType, AssetType assetType, string symbolNative,
            Guid? logoGuidId, CancellationToken cancellationToken);
    }
}
