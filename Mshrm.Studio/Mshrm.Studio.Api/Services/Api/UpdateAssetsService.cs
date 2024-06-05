using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class UpdateAssetsService : IUpdateAssetsService
    {
        private readonly IAssetsClient _assetsClient;
        private readonly IFileClient _fileClient;

        public UpdateAssetsService(IAssetsClient assetsClient, IFileClient fileClient)
        {
            _assetsClient = assetsClient;
            _fileClient = fileClient;
        }

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
        public async Task<AssetDto> UpdateAssetAsync(Guid assetGuidId, string name, string? description, string symbolNative, PricingProviderType providerType,
            Clients.Pricing.AssetType assetType, TemporaryFileDto? logo, CancellationToken cancellationToken)
        {
            // Create logo
            ResourceDto? persistedLogo = null;
            if (logo != null)
                persistedLogo = await _fileClient.SaveTemporaryFileAsync(new SaveTemporaryFileDto() { Key = logo.TemporaryKey, FileName = logo.FileName, IsPrivate = false }, cancellationToken);

            // Update
            return await _assetsClient.UpdateSupportedAssetAsync(assetGuidId, new UpdateSupportedAssetDto()
            { 
                AssetType = assetType,
                Name = name,
                Description = description,
                SymbolNative = symbolNative,
                ProviderType = providerType,
                LogoGuidId = persistedLogo?.GuidId
            },cancellationToken);
        }
    }
}
