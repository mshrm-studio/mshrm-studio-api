using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.Assets
{
    public class UpdateAssetDto
    {
        [JsonProperty("name")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public string Name { get; private set; }

        [JsonProperty("symbolNative")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public string SymbolNative { get; set; }

        [JsonProperty("description")]
        [StringLength(1000, ErrorMessage = "{0} must have less than {1} characters")]
        public string? Description { get; set; }

        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        [JsonProperty("assetType")]
        public AssetType AssetType { get; set; }

        [JsonProperty("decimalPlaces")]
        public int DecimalPlaces { get; set; }

        [JsonProperty("logo")]
        public TemporaryFileDto? Logo { get; set; }
    }
}
