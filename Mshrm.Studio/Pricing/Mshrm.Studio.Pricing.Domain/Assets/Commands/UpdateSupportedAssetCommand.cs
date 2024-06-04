using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands
{
    public class UpdateSupportedAssetCommand : IRequest<Asset>
    {
        /// <summary>
        /// The asset name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The native symbol ie. USD -> $
        /// </summary>
        public required string SymbolNative { get; set; }

        /// <summary>
        /// The description for the asset
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The provider for asset import
        /// </summary>
        public PricingProviderType ProviderType { get; set; }

        /// <summary>
        /// The type of asset
        /// </summary>
        public AssetType AssetType { get; set; }

        /// <summary>
        /// The supported assets logo GUID
        /// </summary>
        public Guid? LogoGuidId { get; set; }

        /// <summary>
        /// The asset to update
        /// </summary>
        public Guid AssetId { get; set; }
    }
}
