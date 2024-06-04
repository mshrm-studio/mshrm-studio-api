using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IUpdateAssetsService
    {
        /// <summary>
        /// Update a asset
        /// </summary>
        /// <param name="assetGuidId">The asset to update</param>
        /// <param name="name">The new name</param>
        /// <param name="description">A new description</param>
        /// <param name="symbolNative">The new display symbol</param>
        /// <param name="providerType">The new provider to import prices from for this asset</param>
        /// <param name="assetType">The new type</param>
        /// <param name="logo">A new logo - not updated if left null</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated asset</returns>
        public Task<AssetDto> UpdateAssetAsync(Guid assetGuidId, string name, string? description, string symbolNative, PricingProviderType providerType,
            Clients.Pricing.AssetType assetType, TemporaryFileDto? logo, CancellationToken cancellationToken);
    }
}
