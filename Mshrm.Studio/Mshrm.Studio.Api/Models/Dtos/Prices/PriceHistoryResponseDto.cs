using Mshrm.Studio.Api.Models.Dtos.Assets;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Prices
{
    public class PriceHistoryResponseDto
    {
        [JsonProperty("oldPrice")]
        public decimal OldPrice { get; set; }

        [JsonProperty("oldMarketCap")]
        public decimal? OldMarketCap { get; set; }

        [JsonProperty("oldVolume")]
        public decimal? OldVolume { get; set; }

        [JsonProperty("newPrice")]
        public decimal NewPrice { get; set; }

        [JsonProperty("newMarketCap")]
        public decimal? NewMarketCap { get; set; }

        [JsonProperty("newVolume")]
        public decimal? NewVolume { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
