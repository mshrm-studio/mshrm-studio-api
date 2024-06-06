using Mshrm.Studio.Api.Models.Dtos.Assets;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Prices
{
    public class PriceResponseDto
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("marketCap")]
        public decimal? MarketCap { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("baseAsset")]
        public AssetResponseDto BaseAsset { get; set; }

        [JsonProperty("asset")]
        public AssetResponseDto Asset { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }
}
