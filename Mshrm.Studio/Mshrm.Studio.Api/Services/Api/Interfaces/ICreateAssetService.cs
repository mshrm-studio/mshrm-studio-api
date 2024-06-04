using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateAssetService
    {
        /// <summary>
        /// Create a new asset
        /// </summary>
        /// <param name="logo">The logo for the asset</param>
        /// <param name="name">The name</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="symbolNative">Display symbol</param>
        /// <param name="description">A description</param>
        /// <param name="assetType">The type</param>
        /// <param name="providerType">The provider to import asset price</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new asset</returns>
        public Task<AssetDto> CreateAssetAsync(TemporaryFileDto? logo, string name, string symbol, string symbolNative, string description, AssetType assetType,
            PricingProviderType providerType, CancellationToken cancellationToken);
    }
}
