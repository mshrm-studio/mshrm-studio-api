using Azure.Core;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using AssetType = Mshrm.Studio.Api.Clients.Pricing.AssetType;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateAssetService : ICreateAssetService
    {
        private readonly IAssetsClient _assetsClient;
        private readonly IFileClient _fileClient;

        public CreateAssetService(IAssetsClient assetsClient, IFileClient fileClient)
        {
            _assetsClient = assetsClient;
            _fileClient = fileClient;
        }

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
        public async Task<AssetDto> CreateAssetAsync(TemporaryFileRequestDto? logo, string name, string symbol, string symbolNative, string description, AssetType assetType,
            PricingProviderType providerType, CancellationToken cancellationToken)
        {
            // Create logo
            ResourceDto? persistedLogo = null;
            if (logo != null)
                persistedLogo = await _fileClient.SaveTemporaryFileAsync(new SaveTemporaryFileDto() { Key = logo.TemporaryKey, FileName = logo.FileName, IsPrivate = false }, cancellationToken) ;

            // Create asset
            return await _assetsClient.CreateSupportedAssetAsync(new CreateSupportedAssetDto()
            {
                AssetType = assetType,
                Description = description,
                Symbol = symbol,
                SymbolNative = symbolNative,
                Name = name,
                ProviderType = providerType,
                LogoGuidId = persistedLogo?.GuidId
            }, cancellationToken);
        }
    }
}
