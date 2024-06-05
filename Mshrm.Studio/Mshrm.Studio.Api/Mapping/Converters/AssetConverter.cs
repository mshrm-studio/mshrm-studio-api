using AutoMapper;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Assets;
using System.Runtime.Intrinsics;

namespace Mshrm.Studio.Api.Mapping.Converters
{
    /// <summary>
    /// For the conversion of assetDTO to pricingAssetDTO
    /// </summary>
    public class AssetConverter : ITypeConverter<AssetDto, AssetResponseDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetConverter"/> class.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public AssetConverter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Convert a AssetDto to a AssetResponseDto
        /// </summary>
        /// <param name="parent">The AssetDto</param>
        /// <param name="target">The AssetResponseDto</param>
        /// <param name="context">The request context</param>
        /// <returns>A AssetResponseDto</returns>
        public AssetResponseDto Convert(
            AssetDto parent,
            AssetResponseDto target,
            ResolutionContext context)
        {
            target = new AssetResponseDto();

            // Build the url
            var url = (parent.LogoGuidId.HasValue) ? $"https://{_httpContextAccessor.HttpContext?.Request.Host}/api/v1/files/guid/{parent.LogoGuidId}" : null;

            // Set the rest of the properties
            target.CreatedDate = parent.CreatedDate;
            target.Description = parent.Description;
            target.Active = parent.Active;
            target.AssetType = parent.AssetType;
            target.ProviderType = parent.ProviderType;
            target.GuidId = parent.GuidId;
            target.Name = parent.Name;
            target.Symbol = parent.Symbol;
            target.SymbolNative = parent.SymbolNative;
            target.logoGuidId = parent.LogoGuidId;
            target.LogoUrl = url;

            // Return mapped target
            return target;
        }
    }
}
