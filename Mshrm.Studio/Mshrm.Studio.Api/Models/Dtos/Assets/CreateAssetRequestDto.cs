using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Assets
{
    public class CreateAssetRequestDto
    {
        [JsonProperty("name")]
        public string Name {  get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("symbolNative")]
        public string SymbolNative { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        [JsonProperty("assetType")]
        public AssetType AssetType { get; set; }

        [JsonProperty("decimalPlaces")]
        public int DecimalPlaces { get; set; }

        [JsonProperty("logo")]
        public TemporaryFileRequestDto? Logo {  get; set; } 
    }
}
