using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Assets
{
    public class ProviderAssetResponseDto
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
