using Mshrm.Studio.Pricing.Api.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Pricing.Api.Models.Dtos.Asset
{
    /// <summary>
    /// Update model for asset
    /// </summary>
    public class UpdateSupportedAssetDto
    {
        /// <summary>
        /// The asset name
        /// </summary>
        [JsonProperty("name")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string Name { get; set; }

        /// <summary>
        /// The native symbol ie. USD -> $
        /// </summary>
        [JsonProperty("symbolNative")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string SymbolNative { get; set; }

        /// <summary>
        /// The description for the asset
        /// </summary>
        [JsonProperty("description")]
        [StringLength(1000, ErrorMessage = "{0} must have less than {1} characters")]
        public string? Description { get; set; }

        /// <summary>
        /// THe provider for asset import
        /// </summary>
        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        /// <summary>
        /// The type of asset
        /// </summary>
        [JsonProperty("assetType")]
        public AssetType AssetType { get; set; }

        /// <summary>
        /// The supported assets logo GUID
        /// </summary>
        [JsonProperty("logoGuidId")]
        public Guid? LogoGuidId { get; set; }

        [JsonProperty("decimalPlaces")]
        public int DecimalPlaces { get; set; }
    }
}
