using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.Assets
{
    public class CreateAssetRequestDto
    {
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("name")]
        public required string Name {  get; set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("symbol")]
        public required string Symbol { get; set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("symbolNative")]
        public required string SymbolNative { get; set; }

        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("description")]
        public required string Description { get; set; }

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
        public TemporaryFileRequestDto? Logo {  get; set; } 
    }
}
