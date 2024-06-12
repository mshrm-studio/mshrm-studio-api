using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.Assets
{
    public class UpdateAssetRequestDto
    {
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("name")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string Name { get; private set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("symbolNative")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string SymbolNative { get; set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("description")]
        [StringLength(1000, ErrorMessage = "{0} must have less than {1} characters")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("providerType")]
        public required PricingProviderType ProviderType { get; set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("assetType")]
        public required AssetType AssetType { get; set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("decimalPlaces")]
        public required int DecimalPlaces { get; set; }

        [JsonProperty("logo")]
        public TemporaryFileRequestDto? Logo { get; set; }
    }
}
