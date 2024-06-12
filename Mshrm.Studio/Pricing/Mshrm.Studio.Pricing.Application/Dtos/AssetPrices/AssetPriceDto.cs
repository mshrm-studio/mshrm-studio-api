using Mshrm.Studio.Pricing.Api.Models.Dtos.Asset;
using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.Dtos.AssetPrices
{
    public class AssetPriceDto
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("marketCap")]
        public decimal? MarketCap { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("baseAsset")]
        public AssetDto BaseAsset { get; set; }

        [JsonProperty("asset")]
        public AssetDto Asset { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }
}
